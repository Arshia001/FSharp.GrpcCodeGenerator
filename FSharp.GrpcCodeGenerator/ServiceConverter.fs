module ServiceConverter

let methodName (method: Method) = method.Name.Value

let serviceIndex (svc: Service, ctx: FileContext) =
    ctx.File.Service
    |> Seq.indexed
    |> Seq.filter (snd >> ((=) svc))
    |> Seq.head
    |> fst

let serviceFullName (ctx: FileContext, svc: Service) = $"{ctx.File.Package.Value}.{svc.Name.Value}"

let serviceClassName (svc: Service) = svc.Name.Value

let clientClassName (svc: Service) = serviceClassName svc + "Client"

let serverClassName (svc: Service) = serviceClassName svc + "Base"

let methodType (method: Method) =
    match method.ClientStreaming with
    | ValueSome true ->
        match method.ServerStreaming with
        | ValueSome true -> BiDiStreaming
        | _ -> ClientStreaming
    | _ ->
        match method.ServerStreaming with
        | ValueSome true -> ServerStreaming
        | _ -> NoStreaming

let methodTypeName (t: MethodType) =
    match t with
    | NoStreaming -> "global.Grpc.Core.MethodType.Unary"
    | ServerStreaming -> "global.Grpc.Core.MethodType.ServerStreaming"
    | ClientStreaming -> "global.Grpc.Core.MethodType.ClientStreaming"
    | BiDiStreaming -> "global.Grpc.Core.MethodType.DuplexStreaming"

let serverMethodTypeName (t: MethodType) =
    match t with
    | NoStreaming -> "global.Grpc.Core.UnaryServerMethod"
    | ServerStreaming -> "global.Grpc.Core.ClientStreamingServerMethod"
    | ClientStreaming -> "global.Grpc.Core.ServerStreamingServerMethod"
    | BiDiStreaming -> "global.Grpc.Core.DuplexStreamingServerMethod"

let serviceNameFieldName () = "__ServiceName"

let marshallerFieldName (typeName: string) =
    $"__Marshaller_{typeName.Replace ('.', '_')}"

let methodFieldName (method: Method) = $"__Method_{method.Name.Value}"

let protobufTypeNameToFSharpTypeName (ctx: FileContext, name: string) =
    let t = Helpers.findMessageType ctx name
    Helpers.qualifiedInnerNameFromMessages (t.Message.Name.Value, t.ContainerMessages, t.File)

let methodRequestParamMaybe (ctx: FileContext, method: Method, invocationParam: bool) =
    if method.ClientStreaming = ValueSome true
    then ""
    elif invocationParam
    then "request, "
    else $"request: {protobufTypeNameToFSharpTypeName (ctx, method.InputType.Value)}, "

let getMethodInOutTypes (ctx: FileContext, method: Method) =
    let inputType = lazy protobufTypeNameToFSharpTypeName (ctx, method.InputType.Value)
    let outputType = lazy protobufTypeNameToFSharpTypeName (ctx, method.OutputType.Value)
    inputType, outputType

let methodReturnTypeClient (ctx: FileContext, method: Method) =
    let inputType, outputType = getMethodInOutTypes (ctx, method)
    match methodType method with
    | NoStreaming -> $"global.Grpc.Core.AsyncUnaryCall<{outputType.Value}>"
    | ClientStreaming -> $"global.Grpc.Core.AsyncClientStreamingCall<{inputType.Value}, {outputType.Value}>"
    | ServerStreaming -> $"global.Grpc.Core.AsyncServerStreamingCall<{outputType.Value}>"
    | BiDiStreaming -> $"global.Grpc.Core.AsyncDuplexStreamingCall<{inputType.Value}, {outputType.Value}>"

let methodRequestParamServer (ctx: FileContext, method: Method) =
    let inputType, _ = getMethodInOutTypes (ctx, method)
    match methodType method with
    | NoStreaming
    | ServerStreaming -> $"request: {inputType.Value}"
    | ClientStreaming
    | BiDiStreaming -> $"requestStream: global.Grpc.Core.IAsyncStreamReader<{protobufTypeNameToFSharpTypeName (ctx, inputType.Value)}>"

let methodReturnTypeServer (ctx: FileContext, method: Method) =
    let _, outputType= getMethodInOutTypes (ctx, method)
    match methodType method with
    | NoStreaming
    | ClientStreaming -> $"global.System.Threading.Tasks.Task<{outputType.Value}>"
    | ServerStreaming
    | BiDiStreaming -> "global.System.Threading.Tasks.Task"

let methodResponseStreamMaybe (ctx: FileContext, method: Method) =
    let _, outputType= getMethodInOutTypes (ctx, method)
    match methodType method with
    | NoStreaming
    | ClientStreaming -> ""
    | ServerStreaming
    | BiDiStreaming -> $"* responseStream: global.Grpc.Core.IServerStreamWriter<{outputType.Value}>"

let usedMessages (svc: Service) =
    svc.Method
    |> Seq.map (fun m -> [ m.InputType; m.OutputType ])
    |> Seq.concat
    |> Seq.choose (function ValueSome x -> Some x | _ -> None)
    |> Set.ofSeq

let writeMarshallerFields (ctx: FileContext, svc: Service) =
    let usedMessages = usedMessages svc
    if Set.count usedMessages > 0
    then
        // All F# message types currently implement IBufferMessage, and interop with C# messages is not supported anyway
        ctx.Writer.WriteLine "let private __Helper_SerializeMessage<'t when 't :> global.Google.Protobuf.IBufferMessage>(message: 't, ctx: global.Grpc.Core.SerializationContext) ="
        ctx.Writer.Indent()
        ctx.Writer.WriteLines [
            "ctx.SetPayloadLength(message.CalculateSize())"
            "global.Google.Protobuf.MessageExtensions.WriteTo(message :> global.Google.Protobuf.IBufferMessage, ctx.GetBufferWriter())"
            "ctx.Complete()"
        ]
        ctx.Writer.Outdent()

        ctx.Writer.WriteLine "let private __Helper_DeserializeMessage<'t when 't :> global.Google.Protobuf.IMessage<'t>>\
            (ctx: global.Grpc.Core.DeserializationContext, parser: global.Google.Protobuf.MessageParser<'t>) : 't ="
        ctx.Writer.Indent()
        ctx.Writer.WriteLine "parser.ParseFrom(ctx.PayloadAsReadOnlySequence())"
        ctx.Writer.Outdent()

        for msgType in usedMessages do
            let typeName = protobufTypeNameToFSharpTypeName (ctx, msgType)
            ctx.Writer.WriteLine $"let private {marshallerFieldName typeName}: global.Grpc.Core.Marshaller<{typeName}> = \
                global.Grpc.Core.Marshallers.Create(global.System.Action<_,_>(fun msg ctx -> __Helper_SerializeMessage(msg, ctx)), \
                global.System.Func<_,_>(fun ctx -> __Helper_DeserializeMessage(ctx, {typeName}.Parser)))"

let writeStaticMethodField (ctx: FileContext, method: Method) =
    let inputType, outputType = getMethodInOutTypes (ctx, method)
    ctx.Writer.WriteLine $"let private {methodFieldName method} = global.Grpc.Core.Method<{inputType.Value}, {outputType.Value}>(\
        {methodTypeName(methodType method)}, {serviceNameFieldName()}, \"{methodName method}\", \
        {marshallerFieldName inputType.Value}, {marshallerFieldName outputType.Value})"

let writeServiceDescriptorProperty (ctx: FileContext, svc: Service) =
    ctx.Writer.WriteLine $"let Descriptor : global.Google.Protobuf.Reflection.ServiceDescriptor = \
        {Helpers.reflectionClassName(ctx.File)}.Descriptor().Services.[{serviceIndex(svc, ctx)}]"

let writeServerClass (ctx: FileContext, svc: Service) =
    // I'd love to make this an interface, but the GRPC runtime only works
    // with classes. See `Grpc.Shared.Server.BindMethodFinder`.
    ctx.Writer.WriteLines [
        $"[<global.Grpc.Core.BindServiceMethod(typeof<{serviceClassName svc}MethodBinder>, \"BindService\")>]"
        "[<AbstractClass>]"
        $"type {serverClassName svc}() ="
    ]
    ctx.Writer.Indent()

    for method in svc.Method do
        ctx.Writer.WriteLine $"abstract {methodName method} : \
            {methodRequestParamServer (ctx, method)}{methodResponseStreamMaybe (ctx, method)} \
            -> context: global.Grpc.Core.ServerCallContext -> {methodReturnTypeServer (ctx, method)}"

    ctx.Writer.Outdent()

type T =
    inherit System.Collections.Generic.List<int>

    new(i: int) = { inherit System.Collections.Generic.List<int>(i) }

let writeClientStub (ctx: FileContext, svc: Service) =
    ctx.Writer.WriteLine $"type {clientClassName svc} ="
    ctx.Writer.Indent()

    ctx.Writer.WriteLines [
        $"inherit global.Grpc.Core.ClientBase<{clientClassName svc}>"
        $"new(channel: global.Grpc.Core.ChannelBase) = {{ inherit global.Grpc.Core.ClientBase<{clientClassName svc}>(channel) }}"
        $"new(callInvoker: global.Grpc.Core.CallInvoker) = {{ inherit global.Grpc.Core.ClientBase<{clientClassName svc}>(callInvoker) }}"
        $"new(configuration: global.Grpc.Core.ClientBase.ClientBaseConfiguration) = {{ inherit global.Grpc.Core.ClientBase<{clientClassName svc}>(configuration) }}"
    ]

    for method in svc.Method do
        let methodType = methodType method
        let inputType, outputType = getMethodInOutTypes (ctx, method)
        if methodType = NoStreaming
        then
            ctx.Writer.WriteLine $"member me.{methodName method}(request: {inputType.Value}, \
                ?headers: global.Grpc.Core.Metadata, ?deadline: global.System.DateTime, \
                ?cancellationToken: global.System.Threading.CancellationToken) : {outputType.Value} ="
            ctx.Writer.Indent()
            ctx.Writer.WriteLine $"me.{methodName method}(request, global.Grpc.Core.CallOptions(\
                defaultArg headers null, Option.toNullable deadline, \
                defaultArg cancellationToken global.System.Threading.CancellationToken.None))"
            ctx.Writer.Outdent()

            ctx.Writer.WriteLine $"member me.{methodName method}(request: {inputType.Value}, \
                callOptions: global.Grpc.Core.CallOptions) : {outputType.Value} ="
            ctx.Writer.Indent()
            ctx.Writer.WriteLine $"me.CallInvoker.BlockingUnaryCall({methodFieldName method}, null, callOptions, request)"
            ctx.Writer.Outdent()

        ctx.Writer.WriteLine $"member me.{methodName method}Async({methodRequestParamMaybe(ctx, method, false)}\
            ?headers: global.Grpc.Core.Metadata, ?deadline: global.System.DateTime, \
            ?cancellationToken: global.System.Threading.CancellationToken) : {methodReturnTypeClient (ctx, method)} ="
        ctx.Writer.Indent()
        ctx.Writer.WriteLine $"me.{methodName method}Async(request, global.Grpc.Core.CallOptions(\
            defaultArg headers null, Option.toNullable deadline, \
            defaultArg cancellationToken global.System.Threading.CancellationToken.None))"
        ctx.Writer.Outdent()

        ctx.Writer.WriteLine $"member me.{methodName method}Async({methodRequestParamMaybe(ctx, method, false)}\
            callOptions: global.Grpc.Core.CallOptions) : {methodReturnTypeClient (ctx, method)} ="
        ctx.Writer.Indent()
        match methodType with
        | NoStreaming -> ctx.Writer.WriteLine $"me.CallInvoker.AsyncUnaryCall({methodFieldName method}, null, callOptions, request)"
        | ClientStreaming -> ctx.Writer.WriteLine $"me.CallInvoker.AsyncClientStreamingCall({methodFieldName method}, null, callOptions)"
        | ServerStreaming -> ctx.Writer.WriteLine $"me.CallInvoker.AsyncServerStreamingCall({methodFieldName method}, null, callOptions, request)"
        | BiDiStreaming -> ctx.Writer.WriteLine $"me.CallInvoker.AsyncDuplexStreamingCall({methodFieldName method}, null, callOptions)"
        ctx.Writer.Outdent()

    ctx.Writer.WriteLine $"override __.NewInstance(configuration: global.Grpc.Core.ClientBase.ClientBaseConfiguration) = \
        {clientClassName svc}(configuration)"

    ctx.Writer.Outdent()

let writeServiceBinderClass (ctx: FileContext, svc: Service) =
    ctx.Writer.WriteLine $"type {serviceClassName svc}MethodBinder ="
    ctx.Writer.Indent()

    ctx.Writer.WriteLine $"static member BindService(serviceImpl: {serverClassName svc}) : global.Grpc.Core.ServerServiceDefinition ="
    ctx.Writer.Indent()
    ctx.Writer.Write $"global.Grpc.Core.ServerServiceDefinition.CreateBuilder()"
    for method in svc.Method do
        ctx.Writer.Write $".AddMethod({methodFieldName method}, serviceImpl.{methodName method})"
    ctx.Writer.WriteLine $".Build()"
    ctx.Writer.Outdent()

    ctx.Writer.WriteLine $"static member BindService(serviceBinder: global.Grpc.Core.ServiceBinderBase, serviceImpl: {serverClassName svc}) ="
    ctx.Writer.Indent()
    for method in svc.Method do
        let inputType, outputType = getMethodInOutTypes (ctx, method)
        ctx.Writer.WriteLine $"serviceBinder.AddMethod({methodFieldName method}, \
            if isNull (box serviceImpl) then null else \
            {serverMethodTypeName (methodType method)}<{inputType.Value}, {outputType.Value}>(\
            serviceImpl.{methodName method}))"
    ctx.Writer.Outdent()
    ctx.Writer.Outdent()

let writeService (ctx: FileContext, svc: Service) =
    if not ctx.Options.ClientServices && not ctx.Options.ServerServices
    then ()
    else

    ctx.Writer.WriteLine $"module {serviceClassName svc} ="
    ctx.Writer.Indent()

    ctx.Writer.WriteLine $"let {serviceNameFieldName ()} : string = \"{serviceFullName (ctx, svc)}\""

    writeMarshallerFields (ctx, svc)

    for method in svc.Method do
        writeStaticMethodField (ctx, method)

    writeServiceDescriptorProperty (ctx, svc)

    if ctx.Options.ServerServices
    then
        writeServiceBinderClass (ctx, svc)
        writeServerClass (ctx, svc)

    if ctx.Options.ClientServices
    then writeClientStub (ctx, svc)

    ctx.Writer.Outdent()
