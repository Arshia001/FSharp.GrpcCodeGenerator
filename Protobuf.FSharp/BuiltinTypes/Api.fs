// <auto-generated>
//     Generated by the F# GRPC code generator. DO NOT EDIT!
//     source: google/protobuf/api.proto
// </auto-generated>
namespace rec Google.Protobuf.FSharp.WellKnownTypes
#nowarn "40"
module ApiReflection =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Api_Descriptor() = Descriptor().MessageTypes.[0]
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Method_Descriptor() = Descriptor().MessageTypes.[1]
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Mixin_Descriptor() = Descriptor().MessageTypes.[2]
    let private descriptorBackingField: global.System.Lazy<global.Google.Protobuf.Reflection.FileDescriptor> =
        let descriptorData = global.System.Convert.FromBase64String("\
            Chlnb29nbGUvcHJvdG9idWYvYXBpLnByb3RvEg9nb29nbGUucHJvdG9idWYaJGdvb2dsZS9wcm90b2J1\
            Zi9zb3VyY2VfY29udGV4dC5wcm90bxoaZ29vZ2xlL3Byb3RvYnVmL3R5cGUucHJvdG8iwQIKA0FwaRIS\
            CgRuYW1lGAEgASgJUgRuYW1lEjEKB21ldGhvZHMYAiADKAsyFy5nb29nbGUucHJvdG9idWYuTWV0aG9k\
            UgdtZXRob2RzEjEKB29wdGlvbnMYAyADKAsyFy5nb29nbGUucHJvdG9idWYuT3B0aW9uUgdvcHRpb25z\
            EhgKB3ZlcnNpb24YBCABKAlSB3ZlcnNpb24SRQoOc291cmNlX2NvbnRleHQYBSABKAsyHi5nb29nbGUu\
            cHJvdG9idWYuU291cmNlQ29udGV4dFINc291cmNlQ29udGV4dBIuCgZtaXhpbnMYBiADKAsyFi5nb29n\
            bGUucHJvdG9idWYuTWl4aW5SBm1peGlucxIvCgZzeW50YXgYByABKA4yFy5nb29nbGUucHJvdG9idWYu\
            U3ludGF4UgZzeW50YXgisgIKBk1ldGhvZBISCgRuYW1lGAEgASgJUgRuYW1lEigKEHJlcXVlc3RfdHlw\
            ZV91cmwYAiABKAlSDnJlcXVlc3RUeXBlVXJsEisKEXJlcXVlc3Rfc3RyZWFtaW5nGAMgASgIUhByZXF1\
            ZXN0U3RyZWFtaW5nEioKEXJlc3BvbnNlX3R5cGVfdXJsGAQgASgJUg9yZXNwb25zZVR5cGVVcmwSLQoS\
            cmVzcG9uc2Vfc3RyZWFtaW5nGAUgASgIUhFyZXNwb25zZVN0cmVhbWluZxIxCgdvcHRpb25zGAYgAygL\
            MhcuZ29vZ2xlLnByb3RvYnVmLk9wdGlvblIHb3B0aW9ucxIvCgZzeW50YXgYByABKA4yFy5nb29nbGUu\
            cHJvdG9idWYuU3ludGF4UgZzeW50YXgiLwoFTWl4aW4SEgoEbmFtZRgBIAEoCVIEbmFtZRISCgRyb290\
            GAIgASgJUgRyb290QnYKE2NvbS5nb29nbGUucHJvdG9idWZCCEFwaVByb3RvUAFaLGdvb2dsZS5nb2xh\
            bmcub3JnL3Byb3RvYnVmL3R5cGVzL2tub3duL2FwaXBiogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldl\
            bGxLbm93blR5cGVzYgZwcm90bzM=")
        global.System.Lazy<_>(
            (fun () ->
                global.Google.Protobuf.Reflection.FileDescriptor.FromGeneratedCode(
                    descriptorData,
                    [|
                        global.Google.Protobuf.FSharp.WellKnownTypes.SourceContextReflection.Descriptor()
                        global.Google.Protobuf.FSharp.WellKnownTypes.TypeReflection.Descriptor()
                    |],
                    new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(
                        null,
                        null,
                        [|
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Protobuf.FSharp.WellKnownTypes.Api>, global.Google.Protobuf.FSharp.WellKnownTypes.Api.Parser, [| "Name"; "Methods"; "Options"; "Version"; "SourceContext"; "Mixins"; "Syntax" |], null, null, null, null)
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Protobuf.FSharp.WellKnownTypes.Method>, global.Google.Protobuf.FSharp.WellKnownTypes.Method.Parser, [| "Name"; "RequestTypeUrl"; "RequestStreaming"; "ResponseTypeUrl"; "ResponseStreaming"; "Options"; "Syntax" |], null, null, null, null)
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Protobuf.FSharp.WellKnownTypes.Mixin>, global.Google.Protobuf.FSharp.WellKnownTypes.Mixin.Parser, [| "Name"; "Root" |], null, null, null, null)
                        |]
                    )
                )
            ),
            true
        )
    let Descriptor(): global.Google.Protobuf.Reflection.FileDescriptor = descriptorBackingField.Value
type Api = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Name: string
    Methods: global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Method>
    Options: global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>
    mutable Version: string
    mutable SourceContext: ValueOption<global.Google.Protobuf.FSharp.WellKnownTypes.SourceContext>
    Mixins: global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Mixin>
    mutable Syntax: global.Google.Protobuf.FSharp.WellKnownTypes.Syntax
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Api = {
        Api._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Api.Name = me.Name
        Api.Methods = me.Methods.Clone()
        Api.Options = me.Options.Clone()
        Api.Version = me.Version
        Api.SourceContext = me.SourceContext |> global.Microsoft.FSharp.Core.ValueOption.map (fun x -> (x :> global.Google.Protobuf.IMessage<global.Google.Protobuf.FSharp.WellKnownTypes.SourceContext>).Clone())
        Api.Mixins = me.Mixins.Clone()
        Api.Syntax = me.Syntax
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Name <> Api.DefaultValue.Name
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Name)
        me.Methods.WriteTo(&output, Api.RepeatedMethodsCodec)
        me.Options.WriteTo(&output, Api.RepeatedOptionsCodec)
        if me.Version <> Api.DefaultValue.Version
        then
            output.WriteRawTag(34uy)
            output.WriteString(me.Version)
        if me.SourceContext <> ValueNone
        then
            output.WriteRawTag(42uy)
            output.WriteMessage(me.SourceContext.Value)
        me.Mixins.WriteTo(&output, Api.RepeatedMixinsCodec)
        if me.Syntax <> Api.DefaultValue.Syntax
        then
            output.WriteRawTag(56uy)
            output.WriteEnum(int me.Syntax)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Name <> Api.DefaultValue.Name then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Name)
        size <- size + me.Methods.CalculateSize(Api.RepeatedMethodsCodec)
        size <- size + me.Options.CalculateSize(Api.RepeatedOptionsCodec)
        if me.Version <> Api.DefaultValue.Version then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Version)
        if me.SourceContext <> ValueNone then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeMessageSize(me.SourceContext.Value)
        size <- size + me.Mixins.CalculateSize(Api.RepeatedMixinsCodec)
        if me.Syntax <> Api.DefaultValue.Syntax then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeEnumSize(int me.Syntax)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Api) =
        if other.Name <> Api.DefaultValue.Name
        then me.Name <- other.Name
        me.Methods.Add(other.Methods)
        me.Options.Add(other.Options)
        if other.Version <> Api.DefaultValue.Version
        then me.Version <- other.Version
        if other.SourceContext <> ValueNone
        then
            if me.SourceContext.IsNone
            then me.SourceContext <- ValueSome(global.Google.Protobuf.FSharp.WellKnownTypes.SourceContext.empty())
            (me.SourceContext.Value :> global.Google.Protobuf.IMessage<global.Google.Protobuf.FSharp.WellKnownTypes.SourceContext>).MergeFrom(other.SourceContext.Value)
        me.Mixins.Add(other.Mixins)
        if other.Syntax <> Api.DefaultValue.Syntax
        then me.Syntax <- other.Syntax
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Name <- input.ReadString()
            | 18u ->
                me.Methods.AddEntriesFrom(&input, Api.RepeatedMethodsCodec)
            | 26u ->
                me.Options.AddEntriesFrom(&input, Api.RepeatedOptionsCodec)
            | 34u ->
                me.Version <- input.ReadString()
            | 42u ->
                if me.SourceContext.IsNone
                then me.SourceContext <- ValueSome(global.Google.Protobuf.FSharp.WellKnownTypes.SourceContext.empty())
                input.ReadMessage(me.SourceContext.Value)
            | 50u ->
                me.Mixins.AddEntriesFrom(&input, Api.RepeatedMixinsCodec)
            | 56u ->
                me.Syntax <- enum(input.ReadEnum())
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Api> with
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
        member __.Descriptor = global.Google.Protobuf.FSharp.WellKnownTypes.ApiReflection.Api_Descriptor()
module Api =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Api._UnknownFields = null
        Api.Name = ""
        Api.Methods = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Method>()
        Api.Options = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>()
        Api.Version = ""
        Api.SourceContext = ValueNone
        Api.Mixins = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Mixin>()
        Api.Syntax = global.Google.Protobuf.FSharp.WellKnownTypes.Syntax.Proto2
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Api._UnknownFields = null
        Api.Name = ""
        Api.Methods = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Method>()
        Api.Options = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>()
        Api.Version = ""
        Api.SourceContext = ValueNone
        Api.Mixins = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Mixin>()
        Api.Syntax = global.Google.Protobuf.FSharp.WellKnownTypes.Syntax.Proto2
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Api>(global.System.Func<_>(empty))
    let NameFieldNumber = 1
    let MethodsFieldNumber = 2
    let OptionsFieldNumber = 3
    let VersionFieldNumber = 4
    let SourceContextFieldNumber = 5
    let MixinsFieldNumber = 6
    let SyntaxFieldNumber = 7
    let RepeatedMethodsCodec = global.Google.Protobuf.FieldCodec.ForMessage(18u, global.Google.Protobuf.FSharp.WellKnownTypes.Method.Parser)
    let RepeatedOptionsCodec = global.Google.Protobuf.FieldCodec.ForMessage(26u, global.Google.Protobuf.FSharp.WellKnownTypes.Option.Parser)
    let RepeatedMixinsCodec = global.Google.Protobuf.FieldCodec.ForMessage(50u, global.Google.Protobuf.FSharp.WellKnownTypes.Mixin.Parser)
type Method = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Name: string
    mutable RequestTypeUrl: string
    mutable RequestStreaming: bool
    mutable ResponseTypeUrl: string
    mutable ResponseStreaming: bool
    Options: global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>
    mutable Syntax: global.Google.Protobuf.FSharp.WellKnownTypes.Syntax
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Method = {
        Method._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Method.Name = me.Name
        Method.RequestTypeUrl = me.RequestTypeUrl
        Method.RequestStreaming = me.RequestStreaming
        Method.ResponseTypeUrl = me.ResponseTypeUrl
        Method.ResponseStreaming = me.ResponseStreaming
        Method.Options = me.Options.Clone()
        Method.Syntax = me.Syntax
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Name <> Method.DefaultValue.Name
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Name)
        if me.RequestTypeUrl <> Method.DefaultValue.RequestTypeUrl
        then
            output.WriteRawTag(18uy)
            output.WriteString(me.RequestTypeUrl)
        if me.RequestStreaming <> Method.DefaultValue.RequestStreaming
        then
            output.WriteRawTag(24uy)
            output.WriteBool(me.RequestStreaming)
        if me.ResponseTypeUrl <> Method.DefaultValue.ResponseTypeUrl
        then
            output.WriteRawTag(34uy)
            output.WriteString(me.ResponseTypeUrl)
        if me.ResponseStreaming <> Method.DefaultValue.ResponseStreaming
        then
            output.WriteRawTag(40uy)
            output.WriteBool(me.ResponseStreaming)
        me.Options.WriteTo(&output, Method.RepeatedOptionsCodec)
        if me.Syntax <> Method.DefaultValue.Syntax
        then
            output.WriteRawTag(56uy)
            output.WriteEnum(int me.Syntax)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Name <> Method.DefaultValue.Name then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Name)
        if me.RequestTypeUrl <> Method.DefaultValue.RequestTypeUrl then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.RequestTypeUrl)
        if me.RequestStreaming <> Method.DefaultValue.RequestStreaming then size <- size + 2
        if me.ResponseTypeUrl <> Method.DefaultValue.ResponseTypeUrl then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.ResponseTypeUrl)
        if me.ResponseStreaming <> Method.DefaultValue.ResponseStreaming then size <- size + 2
        size <- size + me.Options.CalculateSize(Method.RepeatedOptionsCodec)
        if me.Syntax <> Method.DefaultValue.Syntax then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeEnumSize(int me.Syntax)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Method) =
        if other.Name <> Method.DefaultValue.Name
        then me.Name <- other.Name
        if other.RequestTypeUrl <> Method.DefaultValue.RequestTypeUrl
        then me.RequestTypeUrl <- other.RequestTypeUrl
        if other.RequestStreaming <> Method.DefaultValue.RequestStreaming
        then me.RequestStreaming <- other.RequestStreaming
        if other.ResponseTypeUrl <> Method.DefaultValue.ResponseTypeUrl
        then me.ResponseTypeUrl <- other.ResponseTypeUrl
        if other.ResponseStreaming <> Method.DefaultValue.ResponseStreaming
        then me.ResponseStreaming <- other.ResponseStreaming
        me.Options.Add(other.Options)
        if other.Syntax <> Method.DefaultValue.Syntax
        then me.Syntax <- other.Syntax
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Name <- input.ReadString()
            | 18u ->
                me.RequestTypeUrl <- input.ReadString()
            | 24u ->
                me.RequestStreaming <- input.ReadBool()
            | 34u ->
                me.ResponseTypeUrl <- input.ReadString()
            | 40u ->
                me.ResponseStreaming <- input.ReadBool()
            | 50u ->
                me.Options.AddEntriesFrom(&input, Method.RepeatedOptionsCodec)
            | 56u ->
                me.Syntax <- enum(input.ReadEnum())
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Method> with
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
        member __.Descriptor = global.Google.Protobuf.FSharp.WellKnownTypes.ApiReflection.Method_Descriptor()
module Method =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Method._UnknownFields = null
        Method.Name = ""
        Method.RequestTypeUrl = ""
        Method.RequestStreaming = false
        Method.ResponseTypeUrl = ""
        Method.ResponseStreaming = false
        Method.Options = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>()
        Method.Syntax = global.Google.Protobuf.FSharp.WellKnownTypes.Syntax.Proto2
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Method._UnknownFields = null
        Method.Name = ""
        Method.RequestTypeUrl = ""
        Method.RequestStreaming = false
        Method.ResponseTypeUrl = ""
        Method.ResponseStreaming = false
        Method.Options = global.Google.Protobuf.Collections.RepeatedField<global.Google.Protobuf.FSharp.WellKnownTypes.Option>()
        Method.Syntax = global.Google.Protobuf.FSharp.WellKnownTypes.Syntax.Proto2
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Method>(global.System.Func<_>(empty))
    let NameFieldNumber = 1
    let RequestTypeUrlFieldNumber = 2
    let RequestStreamingFieldNumber = 3
    let ResponseTypeUrlFieldNumber = 4
    let ResponseStreamingFieldNumber = 5
    let OptionsFieldNumber = 6
    let SyntaxFieldNumber = 7
    let RepeatedOptionsCodec = global.Google.Protobuf.FieldCodec.ForMessage(50u, global.Google.Protobuf.FSharp.WellKnownTypes.Option.Parser)
type Mixin = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Name: string
    mutable Root: string
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Mixin = {
        Mixin._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Mixin.Name = me.Name
        Mixin.Root = me.Root
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Name <> Mixin.DefaultValue.Name
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Name)
        if me.Root <> Mixin.DefaultValue.Root
        then
            output.WriteRawTag(18uy)
            output.WriteString(me.Root)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Name <> Mixin.DefaultValue.Name then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Name)
        if me.Root <> Mixin.DefaultValue.Root then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Root)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Mixin) =
        if other.Name <> Mixin.DefaultValue.Name
        then me.Name <- other.Name
        if other.Root <> Mixin.DefaultValue.Root
        then me.Root <- other.Root
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Name <- input.ReadString()
            | 18u ->
                me.Root <- input.ReadString()
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Mixin> with
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
        member __.Descriptor = global.Google.Protobuf.FSharp.WellKnownTypes.ApiReflection.Mixin_Descriptor()
module Mixin =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Mixin._UnknownFields = null
        Mixin.Name = ""
        Mixin.Root = ""
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Mixin._UnknownFields = null
        Mixin.Name = ""
        Mixin.Root = ""
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Mixin>(global.System.Func<_>(empty))
    let NameFieldNumber = 1
    let RootFieldNumber = 2
// End of auto-generated code
