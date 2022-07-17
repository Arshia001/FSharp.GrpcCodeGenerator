[<AutoOpen>]
module TypeAliases

open Google.Protobuf.FSharp

type FSWriter = IFSharpFileWriter

type Enum = Reflection.EnumDescriptorProto
type ProtoEnum = Reflection.EnumDescriptorProto // For disambiguation with System.Enum
type File = Reflection.FileDescriptorProto
type Field = Reflection.FieldDescriptorProto
type FieldType = Reflection.FieldDescriptorProto.Types.Type
type FieldLabel = Reflection.FieldDescriptorProto.Types.Label
type Message = Reflection.DescriptorProto
type OneOf = Reflection.OneofDescriptorProto
type Service = Reflection.ServiceDescriptorProto
type Method = Reflection.MethodDescriptorProto

type MethodType =
| NoStreaming
| ClientStreaming
| ServerStreaming
| BiDiStreaming

type Options = {
    InternalAccess: bool
    ServerServices: bool
    ClientServices: bool
}

type FileContext = {
    File: File
    Dependencies: File list
    PublicDependencies: File list
    Writer: FSWriter
    Options: Options
}

type OneOfIsSynthetic = bool

type FSField =
| Single of Field
| OneOf of OneOf * Field list * OneOfIsSynthetic 

type MessageContext = {
    File: FileContext
    ContainerMessages: Message list
    Message: Message
    OrderedFSFields: FSField list
}


module List =
    let init' (l: _ list) = List.take (List.length l - 1) l
