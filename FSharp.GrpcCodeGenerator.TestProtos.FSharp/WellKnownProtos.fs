// <auto-generated>
//     Generated by the F# GRPC code generator. DO NOT EDIT!
//     source: well_known_protos.proto
// </auto-generated>
namespace rec FSharp.GrpcCodeGenerator.TestProtos.FSharp
#nowarn "40"
module WellKnownProtosReflection =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Request_Descriptor() = Descriptor().MessageTypes.[0]
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Response_Descriptor() = Descriptor().MessageTypes.[1]
    let private descriptorBackingField: global.System.Lazy<global.Google.Protobuf.Reflection.FileDescriptor> =
        let descriptorData = global.System.Convert.FromBase64String("\
            Chd3ZWxsX2tub3duX3Byb3Rvcy5wcm90bxIRd2VsbF9rbm93bl9wcm90b3MaHGdvb2dsZS9hcGkvYW5u\
            b3RhdGlvbnMucHJvdG8iIwoHUmVxdWVzdBIYCgdtZXNzYWdlGAEgASgJUgdtZXNzYWdlIiQKCFJlc3Bv\
            bnNlEhgKB21lc3NhZ2UYASABKAlSB21lc3NhZ2UybQoQQW5ub3RhdGVkU2VydmljZRJZCgdEb1N0dWZm\
            Ehoud2VsbF9rbm93bl9wcm90b3MuUmVxdWVzdBobLndlbGxfa25vd25fcHJvdG9zLlJlc3BvbnNlIhWC\
            0+STAg8SDS92MS9ib290c3RyYXBCLaoCKkZTaGFycC5HcnBjQ29kZUdlbmVyYXRvci5UZXN0UHJvdG9z\
            LkZTaGFycGIGcHJvdG8z")
        global.System.Lazy<_>(
            (fun () ->
                global.Google.Protobuf.Reflection.FileDescriptor.FromGeneratedCode(
                    descriptorData,
                    [|
                        global.Google.Api.AnnotationsReflection.Descriptor()
                    |],
                    new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(
                        null,
                        null,
                        [|
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request>, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request.Parser, [| "Message" |], null, null, null, null)
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response>, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response.Parser, [| "Message" |], null, null, null, null)
                        |]
                    )
                )
            ),
            true
        )
    let Descriptor(): global.Google.Protobuf.Reflection.FileDescriptor = descriptorBackingField.Value
type Request = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Message: ValueOption<string>
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Request = {
        Request._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Request.Message = me.Message
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Message <> ValueNone
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Message.Value)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Message <> ValueNone then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Message.Value)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Request) =
        if other.Message <> ValueNone
        then me.Message <- other.Message
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Message <- ValueSome(input.ReadString())
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Request> with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.Clone() = me.Clone()
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.MergeFrom(other) = me.MergeFrom(other)
    interface global.Google.Protobuf.IMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.CalculateSize() = me.CalculateSize()
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.MergeFrom(input) = input.ReadRawMessage(me)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.WriteTo(output) = output.WriteRawMessage(me)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member __.Descriptor = global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.WellKnownProtosReflection.Request_Descriptor()
module Request =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Request._UnknownFields = null
        Request.Message = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Request._UnknownFields = null
        Request.Message = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Request>(global.System.Func<_>(empty))
    let MessageFieldNumber = 1
type Response = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Message: ValueOption<string>
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Response = {
        Response._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Response.Message = me.Message
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Message <> ValueNone
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Message.Value)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Message <> ValueNone then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Message.Value)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Response) =
        if other.Message <> ValueNone
        then me.Message <- other.Message
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Message <- ValueSome(input.ReadString())
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Response> with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.Clone() = me.Clone()
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.MergeFrom(other) = me.MergeFrom(other)
    interface global.Google.Protobuf.IMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.CalculateSize() = me.CalculateSize()
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.MergeFrom(input) = input.ReadRawMessage(me)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.WriteTo(output) = output.WriteRawMessage(me)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member __.Descriptor = global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.WellKnownProtosReflection.Response_Descriptor()
module Response =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Response._UnknownFields = null
        Response.Message = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Response._UnknownFields = null
        Response.Message = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Response>(global.System.Func<_>(empty))
    let MessageFieldNumber = 1
module AnnotatedService =
    let __ServiceName : string = "well_known_protos.AnnotatedService"
    let private __Helper_SerializeMessage<'t when 't :> global.Google.Protobuf.IBufferMessage>(message: 't, ctx: global.Grpc.Core.SerializationContext) =
        ctx.SetPayloadLength(message.CalculateSize())
        global.Google.Protobuf.MessageExtensions.WriteTo(message :> global.Google.Protobuf.IBufferMessage, ctx.GetBufferWriter())
        ctx.Complete()
    let private __Helper_DeserializeMessage<'t when 't :> global.Google.Protobuf.IMessage<'t>>(ctx: global.Grpc.Core.DeserializationContext, parser: global.Google.Protobuf.MessageParser<'t>) : 't =
        parser.ParseFrom(ctx.PayloadAsReadOnlySequence())
    let private __Marshaller_global_FSharp_GrpcCodeGenerator_TestProtos_FSharp_Request: global.Grpc.Core.Marshaller<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request> = global.Grpc.Core.Marshallers.Create(global.System.Action<_,_>(fun msg ctx -> __Helper_SerializeMessage(msg, ctx)), global.System.Func<_,_>(fun ctx -> __Helper_DeserializeMessage(ctx, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request.Parser)))
    let private __Marshaller_global_FSharp_GrpcCodeGenerator_TestProtos_FSharp_Response: global.Grpc.Core.Marshaller<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> = global.Grpc.Core.Marshallers.Create(global.System.Action<_,_>(fun msg ctx -> __Helper_SerializeMessage(msg, ctx)), global.System.Func<_,_>(fun ctx -> __Helper_DeserializeMessage(ctx, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response.Parser)))
    let private __Method_DoStuff = global.Grpc.Core.Method<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response>(global.Grpc.Core.MethodType.Unary, __ServiceName, "DoStuff", __Marshaller_global_FSharp_GrpcCodeGenerator_TestProtos_FSharp_Request, __Marshaller_global_FSharp_GrpcCodeGenerator_TestProtos_FSharp_Response)
    let Descriptor : global.Google.Protobuf.Reflection.ServiceDescriptor = global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.WellKnownProtosReflection.Descriptor().Services.[0]
    type AnnotatedServiceMethodBinder =
        static member BindService(serviceImpl: AnnotatedServiceBase) : global.Grpc.Core.ServerServiceDefinition =
            global.Grpc.Core.ServerServiceDefinition.CreateBuilder().AddMethod(__Method_DoStuff, serviceImpl.DoStuff).Build()
        static member BindService(serviceBinder: global.Grpc.Core.ServiceBinderBase, serviceImpl: AnnotatedServiceBase) =
            serviceBinder.AddMethod(__Method_DoStuff, if isNull (box serviceImpl) then null else global.Grpc.Core.UnaryServerMethod<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response>(serviceImpl.DoStuff))
    [<global.Grpc.Core.BindServiceMethod(typeof<AnnotatedServiceMethodBinder>, "BindService")>]
    [<AbstractClass>]
    type AnnotatedServiceBase() =
        abstract DoStuff : request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request -> context: global.Grpc.Core.ServerCallContext -> global.System.Threading.Tasks.Task<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response>
    type AnnotatedServiceClient =
        inherit global.Grpc.Core.ClientBase<AnnotatedServiceClient>
        new(channel: global.Grpc.Core.ChannelBase) = { inherit global.Grpc.Core.ClientBase<AnnotatedServiceClient>(channel) }
        new(callInvoker: global.Grpc.Core.CallInvoker) = { inherit global.Grpc.Core.ClientBase<AnnotatedServiceClient>(callInvoker) }
        new(configuration: global.Grpc.Core.ClientBase.ClientBaseConfiguration) = { inherit global.Grpc.Core.ClientBase<AnnotatedServiceClient>(configuration) }
        member me.DoStuff(request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, ?headers: global.Grpc.Core.Metadata, ?deadline: global.System.DateTime, ?cancellationToken: global.System.Threading.CancellationToken) : global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response =
            me.DoStuff(request, global.Grpc.Core.CallOptions(defaultArg headers null, Option.toNullable deadline, defaultArg cancellationToken global.System.Threading.CancellationToken.None))
        member me.DoStuff(request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, callOptions: global.Grpc.Core.CallOptions) : global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response =
            me.CallInvoker.BlockingUnaryCall(__Method_DoStuff, null, callOptions, request)
        member me.DoStuffAsync(request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, ?headers: global.Grpc.Core.Metadata, ?deadline: global.System.DateTime, ?cancellationToken: global.System.Threading.CancellationToken) : global.Grpc.Core.AsyncUnaryCall<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> =
            me.DoStuffAsync(request, global.Grpc.Core.CallOptions(defaultArg headers null, Option.toNullable deadline, defaultArg cancellationToken global.System.Threading.CancellationToken.None))
        member me.DoStuffAsync(request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request, callOptions: global.Grpc.Core.CallOptions) : global.Grpc.Core.AsyncUnaryCall<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> =
            me.CallInvoker.AsyncUnaryCall(__Method_DoStuff, null, callOptions, request)
        override __.NewInstance(configuration: global.Grpc.Core.ClientBase.ClientBaseConfiguration) = AnnotatedServiceClient(configuration)
    module AnnotatedServiceClient =
        module Functions =
            let doStuff (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response = client.DoStuff(request)
            let doStuffOptions (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (options: global.Grpc.Core.CallOptions) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response = client.DoStuff(request, options)
            let doStuffHeaders (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (headers: global.Grpc.Core.Metadata) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response = client.DoStuff(request, headers)
            let doStuffAsync (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.Grpc.Core.AsyncUnaryCall<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> = client.DoStuffAsync(request)
            let doStuffOptionsAsync (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (options: global.Grpc.Core.CallOptions) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.Grpc.Core.AsyncUnaryCall<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> = client.DoStuffAsync(request, options)
            let doStuffHeadersAsync (client: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.AnnotatedService.AnnotatedServiceClient) (headers: global.Grpc.Core.Metadata) (request: global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Request) : global.Grpc.Core.AsyncUnaryCall<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.Response> = client.DoStuffAsync(request, headers)
// End of auto-generated code
