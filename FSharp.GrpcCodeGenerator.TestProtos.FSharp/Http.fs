// <auto-generated>
//     Generated by the F# GRPC code generator. DO NOT EDIT!
//     source: google/api/http.proto
// </auto-generated>
namespace rec Google.Api
#nowarn "40"
module HttpReflection =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal Http_Descriptor() = Descriptor().MessageTypes.[0]
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal HttpRule_Descriptor() = Descriptor().MessageTypes.[1]
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal CustomHttpPattern_Descriptor() = Descriptor().MessageTypes.[2]
    let private descriptorBackingField: global.System.Lazy<global.Google.Protobuf.Reflection.FileDescriptor> =
        let descriptorData = global.System.Convert.FromBase64String("\
            ChVnb29nbGUvYXBpL2h0dHAucHJvdG8SCmdvb2dsZS5hcGkieQoESHR0cBIqCgVydWxlcxgBIAMoCzIU\
            Lmdvb2dsZS5hcGkuSHR0cFJ1bGVSBXJ1bGVzEkUKH2Z1bGx5X2RlY29kZV9yZXNlcnZlZF9leHBhbnNp\
            b24YAiABKAhSHGZ1bGx5RGVjb2RlUmVzZXJ2ZWRFeHBhbnNpb24i2gIKCEh0dHBSdWxlEhoKCHNlbGVj\
            dG9yGAEgASgJUghzZWxlY3RvchISCgNnZXQYAiABKAlIAFIDZ2V0EhIKA3B1dBgDIAEoCUgAUgNwdXQS\
            FAoEcG9zdBgEIAEoCUgAUgRwb3N0EhgKBmRlbGV0ZRgFIAEoCUgAUgZkZWxldGUSFgoFcGF0Y2gYBiAB\
            KAlIAFIFcGF0Y2gSNwoGY3VzdG9tGAggASgLMh0uZ29vZ2xlLmFwaS5DdXN0b21IdHRwUGF0dGVybkgA\
            UgZjdXN0b20SEgoEYm9keRgHIAEoCVIEYm9keRIjCg1yZXNwb25zZV9ib2R5GAwgASgJUgxyZXNwb25z\
            ZUJvZHkSRQoTYWRkaXRpb25hbF9iaW5kaW5ncxgLIAMoCzIULmdvb2dsZS5hcGkuSHR0cFJ1bGVSEmFk\
            ZGl0aW9uYWxCaW5kaW5nc0IJCgdwYXR0ZXJuIjsKEUN1c3RvbUh0dHBQYXR0ZXJuEhIKBGtpbmQYASAB\
            KAlSBGtpbmQSEgoEcGF0aBgCIAEoCVIEcGF0aEJqCg5jb20uZ29vZ2xlLmFwaUIJSHR0cFByb3RvUAFa\
            QWdvb2dsZS5nb2xhbmcub3JnL2dlbnByb3RvL2dvb2dsZWFwaXMvYXBpL2Fubm90YXRpb25zO2Fubm90\
            YXRpb25z+AEBogIER0FQSWIGcHJvdG8z")
        global.System.Lazy<_>(
            (fun () ->
                global.Google.Protobuf.Reflection.FileDescriptor.FromGeneratedCode(
                    descriptorData,
                    [|
                    |],
                    new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(
                        null,
                        null,
                        [|
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Api.Http>, global.Google.Api.Http.Parser, [| "Rules"; "FullyDecodeReservedExpansion" |], null, null, null, null)
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Api.HttpRule>, global.Google.Api.HttpRule.Parser, [| "Selector"; "Get"; "Put"; "Post"; "Delete"; "Patch"; "Custom"; "Body"; "ResponseBody"; "AdditionalBindings" |], [| "Pattern" |], null, null, null)
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.Google.Api.CustomHttpPattern>, global.Google.Api.CustomHttpPattern.Parser, [| "Kind"; "Path" |], null, null, null, null)
                        |]
                    )
                )
            ),
            true
        )
    let Descriptor(): global.Google.Protobuf.Reflection.FileDescriptor = descriptorBackingField.Value
type Http = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    Rules: global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>
    mutable FullyDecodeReservedExpansion: bool
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : Http = {
        Http._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        Http.Rules = me.Rules.Clone()
        Http.FullyDecodeReservedExpansion = me.FullyDecodeReservedExpansion
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        me.Rules.WriteTo(&output, Http.RepeatedRulesCodec)
        if me.FullyDecodeReservedExpansion <> Http.DefaultValue.FullyDecodeReservedExpansion
        then
            output.WriteRawTag(16uy)
            output.WriteBool(me.FullyDecodeReservedExpansion)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        size <- size + me.Rules.CalculateSize(Http.RepeatedRulesCodec)
        if me.FullyDecodeReservedExpansion <> Http.DefaultValue.FullyDecodeReservedExpansion then size <- size + 2
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: Http) =
        me.Rules.Add(other.Rules)
        if other.FullyDecodeReservedExpansion <> Http.DefaultValue.FullyDecodeReservedExpansion
        then me.FullyDecodeReservedExpansion <- other.FullyDecodeReservedExpansion
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Rules.AddEntriesFrom(&input, Http.RepeatedRulesCodec)
            | 16u ->
                me.FullyDecodeReservedExpansion <- input.ReadBool()
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<Http> with
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
        member __.Descriptor = global.Google.Api.HttpReflection.Http_Descriptor()
module Http =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        Http._UnknownFields = null
        Http.Rules = global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>()
        Http.FullyDecodeReservedExpansion = false
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        Http._UnknownFields = null
        Http.Rules = global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>()
        Http.FullyDecodeReservedExpansion = false
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<Http>(global.System.Func<_>(empty))
    let RulesFieldNumber = 1
    let FullyDecodeReservedExpansionFieldNumber = 2
    let RepeatedRulesCodec = global.Google.Protobuf.FieldCodec.ForMessage(10u, global.Google.Api.HttpRule.Parser)
type HttpRule = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Selector: string
    mutable Body: string
    AdditionalBindings: global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>
    mutable ResponseBody: string
    mutable Pattern: ValueOption<global.Google.Api.HttpRule.Types.Pattern>
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : HttpRule = {
        HttpRule._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        HttpRule.Selector = me.Selector
        HttpRule.Body = me.Body
        HttpRule.AdditionalBindings = me.AdditionalBindings.Clone()
        HttpRule.ResponseBody = me.ResponseBody
        Pattern = me.Pattern
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Selector <> HttpRule.DefaultValue.Selector
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Selector)
        if me.Body <> HttpRule.DefaultValue.Body
        then
            output.WriteRawTag(58uy)
            output.WriteString(me.Body)
        me.AdditionalBindings.WriteTo(&output, HttpRule.RepeatedAdditionalBindingsCodec)
        if me.ResponseBody <> HttpRule.DefaultValue.ResponseBody
        then
            output.WriteRawTag(98uy)
            output.WriteString(me.ResponseBody)
        match me.Pattern with
        | ValueNone -> ()
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Get x) ->
            output.WriteRawTag(18uy)
            output.WriteString(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Put x) ->
            output.WriteRawTag(26uy)
            output.WriteString(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Post x) ->
            output.WriteRawTag(34uy)
            output.WriteString(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Delete x) ->
            output.WriteRawTag(42uy)
            output.WriteString(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Patch x) ->
            output.WriteRawTag(50uy)
            output.WriteString(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Custom x) ->
            output.WriteRawTag(66uy)
            output.WriteMessage(x)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Selector <> HttpRule.DefaultValue.Selector then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Selector)
        if me.Body <> HttpRule.DefaultValue.Body then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Body)
        size <- size + me.AdditionalBindings.CalculateSize(HttpRule.RepeatedAdditionalBindingsCodec)
        if me.ResponseBody <> HttpRule.DefaultValue.ResponseBody then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.ResponseBody)
        match me.Pattern with
        | ValueNone -> ()
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Get x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Put x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Post x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Delete x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Patch x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(x)
        | ValueSome (global.Google.Api.HttpRule.Types.Pattern.Custom x) -> size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeMessageSize(x)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: HttpRule) =
        if other.Selector <> HttpRule.DefaultValue.Selector
        then me.Selector <- other.Selector
        if other.Body <> HttpRule.DefaultValue.Body
        then me.Body <- other.Body
        me.AdditionalBindings.Add(other.AdditionalBindings)
        if other.ResponseBody <> HttpRule.DefaultValue.ResponseBody
        then me.ResponseBody <- other.ResponseBody
        if other.Pattern <> ValueNone
        then me.Pattern <- other.Pattern
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Selector <- input.ReadString()
            | 58u ->
                me.Body <- input.ReadString()
            | 90u ->
                me.AdditionalBindings.AddEntriesFrom(&input, HttpRule.RepeatedAdditionalBindingsCodec)
            | 98u ->
                me.ResponseBody <- input.ReadString()
            | 18u ->
                let value = input.ReadString()
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Get(value))
            | 26u ->
                let value = input.ReadString()
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Put(value))
            | 34u ->
                let value = input.ReadString()
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Post(value))
            | 42u ->
                let value = input.ReadString()
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Delete(value))
            | 50u ->
                let value = input.ReadString()
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Patch(value))
            | 66u ->
                let value = global.Google.Api.CustomHttpPattern.empty()
                input.ReadMessage(value)
                me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Custom(value))
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.PatternCase =
        match me.Pattern with
        | ValueNone -> 0
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Get _) -> 2
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Put _) -> 3
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Post _) -> 4
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Delete _) -> 5
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Patch _) -> 6
        | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Custom _) -> 8
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.ClearPattern() = me.Pattern <- ValueNone
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Get
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Get x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Get x)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Put
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Put x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Put x)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Post
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Post x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Post x)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Delete
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Delete x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Delete x)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Patch
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Patch x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Patch x)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Custom
        with get() =
            match me.Pattern with
            | ValueSome(global.Google.Api.HttpRule.Types.Pattern.Custom x) -> x
            | _ -> Unchecked.defaultof<_>
        and set(x) =
            me.Pattern <- ValueSome(global.Google.Api.HttpRule.Types.Pattern.Custom x)
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<HttpRule> with
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
        member __.Descriptor = global.Google.Api.HttpReflection.HttpRule_Descriptor()
module HttpRule =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        HttpRule._UnknownFields = null
        HttpRule.Selector = ""
        HttpRule.Body = ""
        HttpRule.AdditionalBindings = global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>()
        HttpRule.ResponseBody = ""
        Pattern = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        HttpRule._UnknownFields = null
        HttpRule.Selector = ""
        HttpRule.Body = ""
        HttpRule.AdditionalBindings = global.Google.Protobuf.Collections.RepeatedField<global.Google.Api.HttpRule>()
        HttpRule.ResponseBody = ""
        Pattern = ValueNone
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<HttpRule>(global.System.Func<_>(empty))
    let SelectorFieldNumber = 1
    let GetFieldNumber = 2
    let PutFieldNumber = 3
    let PostFieldNumber = 4
    let DeleteFieldNumber = 5
    let PatchFieldNumber = 6
    let CustomFieldNumber = 8
    let BodyFieldNumber = 7
    let ResponseBodyFieldNumber = 12
    let AdditionalBindingsFieldNumber = 11
    let RepeatedAdditionalBindingsCodec = global.Google.Protobuf.FieldCodec.ForMessage(90u, global.Google.Api.HttpRule.Parser)
    module Types =
        type Pattern =
        | Get of string
        | Put of string
        | Post of string
        | Delete of string
        | Patch of string
        | Custom of global.Google.Api.CustomHttpPattern
type CustomHttpPattern = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable Kind: string
    mutable Path: string
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : CustomHttpPattern = {
        CustomHttpPattern._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        CustomHttpPattern.Kind = me.Kind
        CustomHttpPattern.Path = me.Path
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.Kind <> CustomHttpPattern.DefaultValue.Kind
        then
            output.WriteRawTag(10uy)
            output.WriteString(me.Kind)
        if me.Path <> CustomHttpPattern.DefaultValue.Path
        then
            output.WriteRawTag(18uy)
            output.WriteString(me.Path)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.Kind <> CustomHttpPattern.DefaultValue.Kind then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Kind)
        if me.Path <> CustomHttpPattern.DefaultValue.Path then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeStringSize(me.Path)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: CustomHttpPattern) =
        if other.Kind <> CustomHttpPattern.DefaultValue.Kind
        then me.Kind <- other.Kind
        if other.Path <> CustomHttpPattern.DefaultValue.Path
        then me.Path <- other.Path
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 10u ->
                me.Kind <- input.ReadString()
            | 18u ->
                me.Path <- input.ReadString()
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<CustomHttpPattern> with
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
        member __.Descriptor = global.Google.Api.HttpReflection.CustomHttpPattern_Descriptor()
module CustomHttpPattern =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        CustomHttpPattern._UnknownFields = null
        CustomHttpPattern.Kind = ""
        CustomHttpPattern.Path = ""
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        CustomHttpPattern._UnknownFields = null
        CustomHttpPattern.Kind = ""
        CustomHttpPattern.Path = ""
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<CustomHttpPattern>(global.System.Func<_>(empty))
    let KindFieldNumber = 1
    let PathFieldNumber = 2
// End of auto-generated code
