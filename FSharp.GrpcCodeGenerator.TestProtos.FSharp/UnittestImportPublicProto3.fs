// <auto-generated>
//     Generated by the F# GRPC code generator. DO NOT EDIT!
//     source: unittest_import_public_proto3.proto
// </auto-generated>
namespace rec FSharp.GrpcCodeGenerator.TestProtos.FSharp
#nowarn "40"
module UnittestImportPublicProto3Reflection =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal PublicImportMessage_Descriptor() = Descriptor().MessageTypes.[0]
    let private descriptorBackingField: global.System.Lazy<global.Google.Protobuf.Reflection.FileDescriptor> =
        let descriptorData = global.System.Convert.FromBase64String("\
            CiN1bml0dGVzdF9pbXBvcnRfcHVibGljX3Byb3RvMy5wcm90bxIYcHJvdG9idWZfdW5pdHRlc3RfaW1w\
            b3J0IiMKE1B1YmxpY0ltcG9ydE1lc3NhZ2USDAoBZRgBIAEoBVIBZUItqgIqRlNoYXJwLkdycGNDb2Rl\
            R2VuZXJhdG9yLlRlc3RQcm90b3MuRlNoYXJwYgZwcm90bzM=")
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
                            new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.PublicImportMessage>, global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.PublicImportMessage.Parser, [| "E" |], null, null, null, null)
                        |]
                    )
                )
            ),
            true
        )
    let Descriptor(): global.Google.Protobuf.Reflection.FileDescriptor = descriptorBackingField.Value
type PublicImportMessage = {
    mutable _UnknownFields: global.Google.Protobuf.UnknownFieldSet
    mutable E: int32
} with
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member me.Clone() : PublicImportMessage = {
        PublicImportMessage._UnknownFields = global.Google.Protobuf.UnknownFieldSet.Clone(me._UnknownFields)
        PublicImportMessage.E = me.E
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalWriteTo(output: byref<global.Google.Protobuf.WriteContext>) =
        if me.E <> PublicImportMessage.DefaultValue.E
        then
            output.WriteRawTag(8uy)
            output.WriteInt32(me.E)
        if not <| isNull me._UnknownFields then me._UnknownFields.WriteTo(&output)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.CalculateSize() =
        let mutable size = 0
        if me.E <> PublicImportMessage.DefaultValue.E then size <- size + 1 + global.Google.Protobuf.CodedOutputStream.ComputeInt32Size(me.E)
        if not <| isNull me._UnknownFields then size <- size + me._UnknownFields.CalculateSize()
        size
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.MergeFrom(other: PublicImportMessage) =
        if other.E <> PublicImportMessage.DefaultValue.E
        then me.E <- other.E
        me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFrom(me._UnknownFields, other._UnknownFields)
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    member private me.InternalMergeFrom(input: byref<global.Google.Protobuf.ParseContext>) =
        let mutable tag = input.ReadTag()
        while tag <> 0u do
            match tag with
            | 8u ->
                me.E <- input.ReadInt32()
            | _ ->
                me._UnknownFields <- global.Google.Protobuf.UnknownFieldSet.MergeFieldFrom(me._UnknownFields, &input)
            tag <- input.ReadTag()
    interface global.Google.Protobuf.IBufferMessage with
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalMergeFrom(ctx) = me.InternalMergeFrom(&ctx)
        [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
        member me.InternalWriteTo(ctx) = me.InternalWriteTo(&ctx)
    interface global.Google.Protobuf.IMessage<PublicImportMessage> with
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
        member __.Descriptor = global.FSharp.GrpcCodeGenerator.TestProtos.FSharp.UnittestImportPublicProto3Reflection.PublicImportMessage_Descriptor()
module PublicImportMessage =
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let internal DefaultValue = {
        PublicImportMessage._UnknownFields = null
        PublicImportMessage.E = 0
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let empty () = {
        PublicImportMessage._UnknownFields = null
        PublicImportMessage.E = 0
    }
    [<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]
    let Parser = global.Google.Protobuf.MessageParser<PublicImportMessage>(global.System.Func<_>(empty))
    let EFieldNumber = 1
// End of auto-generated code
