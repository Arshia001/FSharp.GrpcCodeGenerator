[<AutoOpen>]
module rec FieldConverter

type FSField =
| Single of Field
| OneOf of OneOf * Field list

type FieldWriter = {
    WriteMember: FileContext -> unit
    WriteMemberInit: FileContext -> unit
    WriteCloningCode: FileContext -> unit
    WriteMergingCode: FileContext -> unit
    WriteParsingCode: FileContext -> unit
    WriteOneOfParsingCode: FileContext -> unit
    WriteSerializedSizeCode: FileContext -> unit
    WriteSerializedSizeCodeWithoutCheck: FileContext -> string -> unit
    WriteSerializationCode: FileContext -> unit
    WriteSerializationCodeWithoutCheck: FileContext -> string -> unit
    WriteModuleMembers: FileContext -> unit
    WriteCodecCode: FileContext -> unit
    WriteExtensionCode: FileContext -> unit
    WriteOneOfCase: FileContext -> unit
}

let addDeprecatedFlag (ctx: FileContext, field: Field) =
    match field with
    | { Options = ValueSome { Deprecated = ValueSome true } } -> ctx.Writer.Write "[<global.System.ObsoleteAttribute>]"
    | { Type = ValueSome FieldType.Message; TypeName = ValueSome t } ->
        match (Helpers.findMessageType ctx t).Message.Options with
        | ValueSome { Deprecated = ValueSome true } -> ctx.Writer.Write "[<global.System.ObsoleteAttribute>]"
        | _ -> ()
    | _ -> ()

let addPublicMemberAttributes (ctx: FileContext, field: Field) =
    addDeprecatedFlag (ctx, field)
    Helpers.writeGeneratedCodeAttribute ctx

let oneOfPropertyName (oneOf: OneOf) = Helpers.snakeToPascalCase false oneOf.Name.Value

let propertyName (msg: Message, field: Field) = Helpers.propertyName msg field

let needsOptionType (ctx: FileContext, field: Field) =
    if field.Label = ValueSome FieldLabel.Repeated then false
    elif ctx.File.Syntax = ValueSome "proto2" then true
    elif field.Label = ValueSome FieldLabel.Optional then true
    elif field.Type = ValueSome FieldType.Message || field.Type = ValueSome FieldType.Group then true
    else false

let propertyAccess (ctx: FileContext, msg: Message, field: Field) =
    let propName = propertyName (msg, field)
    if needsOptionType (ctx, field)
    then propName + ".Value"
    else propName

let typeNameWithoutOption (ctx: FileContext, field: Field) =
    match field.Type.Value with
    | FieldType.Enum ->
        let enum = Helpers.findEnumType ctx field.TypeName.Value
        Helpers.qualifiedInnerNameFromMessages (enum.Enum.Name.Value, enum.ContainerMessages, enum.File)
    | FieldType.Message
    | FieldType.Group ->
        let msg = Helpers.findMessageType ctx field.TypeName.Value
        Helpers.qualifiedInnerNameFromMessages (msg.Message.Name.Value, msg.ContainerMessages, msg.File)
    | FieldType.Double -> nameof(float)
    | FieldType.Float -> nameof(float32)
    | FieldType.Int64 -> nameof(int64)
    | FieldType.Uint64 -> nameof(uint64)
    | FieldType.Int32 -> nameof(int32)
    | FieldType.Uint32 -> nameof(uint32)
    | FieldType.Fixed64 -> nameof(uint64)
    | FieldType.Sfixed64 -> nameof(int64)
    | FieldType.Fixed32 -> nameof(uint32)
    | FieldType.Sfixed32 -> nameof(int32)
    | FieldType.Bool -> nameof(bool)
    | FieldType.String -> nameof(string)
    | FieldType.Bytes -> "global.Google.Protobuf.ByteString"
    | FieldType.Sint64 -> nameof(int64)
    | FieldType.Sint32 -> nameof(int32)
    | _ -> failwithf "Unknown field type %A" field.Type

let typeName (ctx: FileContext, field: Field) =
    let typeName = typeNameWithoutOption (ctx, field)
    if needsOptionType (ctx, field)
    then Helpers.makeOptionType typeName
    else typeName

let oneOfTypeNameWithoutOption (oneOf: OneOf, containingType: Message, containerMessages: Message list, file: File) =
    Helpers.qualifiedInnerNameFromMessages (oneOfPropertyName oneOf, containerMessages @ [ containingType ], file)

let oneOfTypeName (oneOf: OneOf, containingType: Message, containerMessages: Message list, file: File) =
    Helpers.makeOptionType <| oneOfTypeNameWithoutOption (oneOf, containingType, containerMessages, file)

let oneOfCaseName (oneOf: OneOf, case: Field, containerType: Message, containerMessages: Message list, file: File) =
    oneOfTypeNameWithoutOption (oneOf, containerType, containerMessages, file) + "." + (propertyName (containerType, case))

let tagSize (field: Field) = Google.Protobuf.CodedOutputStream.ComputeTagSize field.Number.Value

let defaultValueString (field: Field) =
    match field.DefaultValue with
    | ValueNone -> "\"\""
    | ValueSome v -> $"global.System.Text.Encoding.UTF8.GetString(global.System.Convert.FromBase64String(\"{System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(v))}\"))"

let defaultValueBytes (field: Field) =
    match field.DefaultValue with
    | ValueNone -> "global.Google.Protobuf.ByteString.Empty"
    | ValueSome v -> $"global.Google.Protobuf.ByteString.FromBase64(\"{System.Convert.ToBase64String(Helpers.getStringBytes v)}\")"

let defaultValueEnum (ctx: FileContext, field: Field) =
    let enum = Helpers.findEnumType ctx field.TypeName.Value
    let caseName =
        match field.DefaultValue with
        | ValueNone -> enum.Enum.Value.[0].Name.Value
        | ValueSome v -> v
    Helpers.qualifiedInnerNameFromMessages (enum.Enum.Name.Value, enum.ContainerMessages, enum.File) + "." + Helpers.enumValueName (enum.Enum.Name.Value, caseName)
    
let defaultValue (ctx: FileContext, field: Field) =
    let rec helper (ctx: FileContext, field: Field) =
        if needsOptionType (ctx, field)
        then "ValueNone"
        else

        match field.Type.Value with
        | FieldType.Enum -> defaultValueEnum (ctx, field)

        | FieldType.Message
        | FieldType.Group -> "ValueNone"

        | FieldType.String -> defaultValueString field
        | FieldType.Bytes -> defaultValueBytes field
        | FieldType.Bool -> if field.DefaultValue = ValueSome "true" then "true" else "false"
        | FieldType.Double -> if field.DefaultValue.Value.Contains '.' then field.DefaultValue.Value else field.DefaultValue.Value + ".0"
        | FieldType.Float -> field.DefaultValue.Value + "f"
        | FieldType.Int64 -> field.DefaultValue.Value + "L"
        | FieldType.Uint64 -> field.DefaultValue.Value + "UL"
        | FieldType.Int32 -> field.DefaultValue.Value
        | FieldType.Fixed64 -> field.DefaultValue.Value + "UL"
        | FieldType.Fixed32 -> field.DefaultValue.Value + "U"
        | FieldType.Uint32 -> field.DefaultValue.Value + "U"
        | FieldType.Sfixed32 -> field.DefaultValue.Value
        | FieldType.Sfixed64 -> field.DefaultValue.Value + "L"
        | FieldType.Sint32 -> field.DefaultValue.Value
        | FieldType.Sint64 -> field.DefaultValue.Value + "L"
    
        | _ -> failwithf "Unknown field type %A" field.Type

    helper (ctx, field)

let capitalizedTypeName (field: Field) =
    match field.Type.Value with
    | FieldType.Enum -> "Enum"
    | FieldType.Message -> "Message"
    | FieldType.Group -> "Group"
    | FieldType.Double -> "Double"
    | FieldType.Float -> "Float"
    | FieldType.Int64 -> "Int64"
    | FieldType.Uint64 -> "UInt64"
    | FieldType.Int32 -> "Int32"
    | FieldType.Fixed64 -> "Fixed64"
    | FieldType.Fixed32 -> "Fixed32"
    | FieldType.Bool -> "Bool"
    | FieldType.String -> "String"
    | FieldType.Bytes -> "Bytes"
    | FieldType.Uint32 -> "UInt32"
    | FieldType.Sfixed32 -> "SFixed32"
    | FieldType.Sfixed64 -> "SFixed64"
    | FieldType.Sint32 -> "SInt32"
    | FieldType.Sint64 -> "SInt64"
    | _ -> failwithf "Unknown field type %A" field.Type

let hasPropertyCheck (ctx: FileContext, msg: Message, field: Field, identifier: string) =
    if needsOptionType (ctx, field)
    then $"{identifier}.{propertyName (msg, field)} <> ValueNone"
    elif field.Label = ValueSome FieldLabel.Repeated
    then $"{identifier}.{propertyName (msg, field)}.Count <> 0"
    else $"{identifier}.{propertyName (msg, field)} <> {Helpers.messageTypeName msg}.DefaultValue.{propertyName(msg, field)}"

let defaultValueAccessIgnoreOption (ctx: FileContext, field: Field) =
    let maybeZero = field.DefaultValue |> ValueOption.defaultValue "0"

    match field.Type.Value with
    | FieldType.Enum -> defaultValueEnum (ctx, field)

    | FieldType.String -> defaultValueString field
    | FieldType.Bytes -> defaultValueBytes field
    | FieldType.Bool -> if field.DefaultValue = ValueSome "true" then "true" else "false"

    | FieldType.Double -> if maybeZero.Contains '.' then maybeZero else maybeZero + ".0"
    | FieldType.Float -> maybeZero + "f"
    | FieldType.Int64 -> maybeZero + "L"
    | FieldType.Uint64 -> maybeZero + "UL"
    | FieldType.Int32 -> maybeZero
    | FieldType.Fixed64 -> maybeZero + "UL"
    | FieldType.Fixed32 -> maybeZero + "U"
    | FieldType.Uint32 -> maybeZero + "U"
    | FieldType.Sfixed32 -> maybeZero
    | FieldType.Sfixed64 -> maybeZero + "L"
    | FieldType.Sint32 -> maybeZero
    | FieldType.Sint64 -> maybeZero + "L"
        
    | FieldType.Message
    | FieldType.Group -> failwith "Not supported"

    | _ -> failwithf "Unknown field type %A" field.Type

let defaultValueAccess (ctx: FileContext, msg: Message, field: Field) =
    if needsOptionType (ctx, field)
    then "ValueNone"
    else $"{Helpers.messageTypeName msg}.DefaultValue.{propertyName(msg, field)}"

let writeParsingCodeTemplate (ctx: FileContext, field: Field) writeBody =
    if Helpers.isFieldPackable field
    then
        ctx.Writer.Write $"| {Helpers.makeTagWithType (field, Google.Protobuf.WireFormat.WireType.LengthDelimited)}u "
    
    ctx.Writer.WriteLine $"| {Helpers.makeTag field}u ->"
    ctx.Writer.Indent()
    
    writeBody ()
    
    ctx.Writer.Outdent()

let NotImplementedWriter = {
    WriteMember = fun _ -> raise <| System.NotImplementedException()
    WriteMemberInit = fun _ -> raise <| System.NotImplementedException()
    WriteOneOfCase = fun _ -> raise <| System.NotImplementedException()
    WriteSerializedSizeCode = fun _ -> raise <| System.NotImplementedException()
    WriteSerializedSizeCodeWithoutCheck = fun _ _ -> raise <| System.NotImplementedException()
    WriteCloningCode = fun _ -> raise <| System.NotImplementedException()
    WriteMergingCode = fun _ -> raise <| System.NotImplementedException()
    WriteParsingCode = fun _ -> raise <| System.NotImplementedException()
    WriteOneOfParsingCode = fun _ -> raise <| System.NotImplementedException()
    WriteModuleMembers = fun _ -> raise <| System.NotImplementedException()
    WriteSerializationCode = fun _ -> raise <| System.NotImplementedException()
    WriteSerializationCodeWithoutCheck = fun _ _ -> raise <| System.NotImplementedException()
    WriteCodecCode = fun _ -> raise <| System.NotImplementedException()
    WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
}

module PrimitiveFieldConverter =
    let writeMember (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"mutable {propertyName (containingType, field)}: {typeName (ctx, field)}"

    let writeMemberInit (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propertyName (containingType, field)} = {defaultValue (ctx, field)}"

    let writeOneOfCase (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"| {propertyName (containingType, field)} of {typeNameWithoutOption (ctx, field)}"

    let writeSerializedSizeCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        let tagSize = tagSize field
        
        match Helpers.fixedSize (field.Type.Value) with
        | Some size -> ctx.Writer.Write <| string (tagSize + size)
        | None ->
            ctx.Writer.Write
                $"{tagSize} + global.Google.Protobuf.CodedOutputStream.Compute{capitalizedTypeName field}Size({id})"

    let writeSerializedSizeCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.Write $"if {propCheck} then size <- size + "
        writeSerializedSizeCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.WriteLine ""

    let writeCloningCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propName} = me.{propName}"

    let writeMergingCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        let propCheck = hasPropertyCheck (ctx, containingType, field, "other")
        ctx.Writer.WriteLines [
            $"if {propCheck}"
            $"then me.{propName} <- other.{propName}"
        ]

    let writeParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeParsingCodeTemplate (ctx, field) <| fun () ->
            let readingCode =
                if needsOptionType (ctx, field)
                then $"ValueSome(input.Read{capitalizedTypeName field}())"
                else $"input.Read{capitalizedTypeName field}()"
            ctx.Writer.WriteLine $"me.{propertyName (containingType, field)} <- {readingCode}"

    let writeOneOfParsingCode (field: Field) (ctx: FileContext) =
        ctx.Writer.WriteLine $"let value = input.Read{capitalizedTypeName field}()"

    let writeSerializationCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        let tagBytes = Helpers.tagBytes field
        let tagBytesString = tagBytes |> Seq.map (sprintf "%iuy") |> String.concat ", "
        ctx.Writer.WriteLines [
            $"output.WriteRawTag({tagBytesString})"
            $"output.Write{capitalizedTypeName field}({id})"
        ]

    let writeSerializationCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.WriteLines [
            $"if {propCheck}"
            "then"
        ]
        ctx.Writer.Indent()
        writeSerializationCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.Outdent()

    let writeCodecCode (field: Field) (ctx: FileContext) =
        ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.For{capitalizedTypeName field}({Helpers.makeTag field}u, {defaultValueAccessIgnoreOption (ctx, field)})"

    let writeExtensionCode (field: Field) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)
        
        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.Extension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "
           
        writeCodecCode (field) ctx
        ctx.Writer.WriteLine ")"

    let create (field: Field, containingType: Message option) =
        match containingType with
        | Some t ->
            {
                WriteMember = writeMember (field, t)
                WriteMemberInit = writeMemberInit (field, t)
                WriteOneOfCase = writeOneOfCase (field, t)
                WriteSerializedSizeCode = writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = writeCloningCode (field, t)
                WriteMergingCode = writeMergingCode (field, t)
                WriteParsingCode = writeParsingCode (field, t)
                WriteOneOfParsingCode = writeOneOfParsingCode field
                WriteSerializationCode = writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = ignore
                WriteCodecCode = writeCodecCode (field)
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field)
            }

module RepeatedPrimitiveFieldConverter =
    let writeMember (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{propertyName (containingType, field)}: global.Google.Protobuf.Collections.RepeatedField<{typeName (ctx, field)}>"

    let writeMemberInit (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propertyName (containingType, field)} = global.Google.Protobuf.Collections.RepeatedField<{typeName (ctx, field)}>()"

    let writeOneOfCase (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"| {propertyName (containingType, field)} of global.Google.Protobuf.Collections.RepeatedField<{typeName (ctx, field)}>"

    let writeSerializedSizeCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        ctx.Writer.Write $"{id}.CalculateSize({Helpers.messageTypeName containingType}.Repeated{propertyName (containingType, field)}Codec)"

    let writeSerializedSizeCode (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.Write "size <- size + "
        writeSerializedSizeCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.WriteLine ""

    let writeCloningCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propName} = me.{propName}.Clone()"

    let writeMergingCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"me.{propName}.Add(other.{propName})"

    let writeParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeParsingCodeTemplate (ctx, field) <| fun () ->
            ctx.Writer.WriteLine $"me.{propertyName (containingType, field)}.AddEntriesFrom(&input, {Helpers.messageTypeName containingType}.Repeated{propertyName (containingType, field)}Codec)"

    let writeOneOfParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLines [
            $"let value = global.Google.Protobuf.Collections.RepeatedField<{typeName (ctx, field)}>()"
            $"value.AddEntriesFrom(&input, {Helpers.messageTypeName containingType}.Repeated{propertyName (containingType, field)}Codec)"
        ]

    let writeSerializationCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        ctx.Writer.WriteLine $"{id}.WriteTo(&output, {Helpers.messageTypeName containingType}.Repeated{propertyName (containingType, field)}Codec)"

    let writeSerializationCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeSerializationCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"

    let writeCodecCode (field: Field) (ctx: FileContext) =
        ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.For{capitalizedTypeName field}({Helpers.makeTag field}u)"
    
    let writeExtensionCode (field: Field) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)
        
        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.RepeatedExtension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "

        writeCodecCode field ctx
        ctx.Writer.WriteLine ")"

    let writeModuleMembers (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.Write $"let Repeated{propertyName (containingType, field)}Codec = "
        writeCodecCode field ctx
        ctx.Writer.WriteLine ""

    let create (field: Field, containingType: Message option) =
        match containingType with
        | Some t ->
            {
                WriteMember = writeMember (field, t)
                WriteMemberInit = writeMemberInit (field, t)
                WriteOneOfCase = writeOneOfCase (field, t)
                WriteSerializedSizeCode = writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = writeCloningCode (field, t)
                WriteMergingCode = writeMergingCode (field, t)
                WriteParsingCode = writeParsingCode (field, t)
                WriteOneOfParsingCode = writeOneOfParsingCode (field, t)
                WriteSerializationCode = writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = writeModuleMembers (field, t)
                WriteCodecCode = writeCodecCode field
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field)
            }

module EnumFieldConverter =
    let writeSerializedSizeCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        let tagSize = tagSize field
        ctx.Writer.Write $"{tagSize} + global.Google.Protobuf.CodedOutputStream.Compute{capitalizedTypeName field}Size(int {id})"

    let writeSerializedSizeCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.Write $"if {propCheck} then size <- size + "
        writeSerializedSizeCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.WriteLine $""

    let writeParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeParsingCodeTemplate (ctx, field) <| fun () ->
            let readingCode =
                if needsOptionType (ctx, field)
                then "ValueSome(enum(input.ReadEnum()))"
                else "enum(input.ReadEnum())"
            ctx.Writer.WriteLine $"me.{propertyName (containingType, field)} <- {readingCode}"

    let writeOneOfParsingCode (field: Field) (ctx: FileContext) =
        ctx.Writer.WriteLine $"let value = enum(input.ReadEnum()) : {typeNameWithoutOption (ctx, field)}"

    let writeSerializationCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        let tagBytes = Helpers.tagBytes field
        let tagBytesString = tagBytes |> Seq.map (sprintf "%iuy") |> String.concat ", "
        ctx.Writer.WriteLines [
            $"output.WriteRawTag({tagBytesString})"
            $"output.Write{capitalizedTypeName field}(int {id})"
        ]

    let writeSerializationCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.WriteLines [
            $"if {propCheck}"
            "then"
        ]
        ctx.Writer.Indent()
        writeSerializationCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.Outdent()

    let writeCodecCode (field: Field) (ctx: FileContext) =
        ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.ForEnum({Helpers.makeTag field}u, global.System.Func<_,_>(fun x -> int x), global.System.Func<_,_>(fun x -> enum x), {defaultValueAccessIgnoreOption (ctx, field)})"

    let writeExtensionCode (field: Field) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)

        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.Extension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "

        writeCodecCode (field) ctx
        ctx.Writer.WriteLine ")"

    let create (field: Field, containingType: Message option) =
        match containingType with
        | Some t ->
            {
                WriteMember = PrimitiveFieldConverter.writeMember (field, t)
                WriteMemberInit = PrimitiveFieldConverter.writeMemberInit (field, t)
                WriteOneOfCase = PrimitiveFieldConverter.writeOneOfCase (field, t)
                WriteSerializedSizeCode = writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = PrimitiveFieldConverter.writeCloningCode (field, t)
                WriteMergingCode = PrimitiveFieldConverter.writeMergingCode (field, t)
                WriteParsingCode = writeParsingCode (field, t)
                WriteOneOfParsingCode = writeOneOfParsingCode field
                WriteSerializationCode = writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = ignore
                WriteCodecCode = writeCodecCode(field)
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field)
            }

module RepeatedEnumFieldConverter =
    let writeCodecCode (field: Field) (ctx: FileContext) =
        ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.ForEnum({Helpers.makeTag field}u, global.System.Func<_,_>(fun x -> int x), global.System.Func<_,_>(fun x -> enum x))"

    let writeExtensionCode (field: Field) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)

        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.RepeatedExtension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "
            
        writeCodecCode field ctx    
        ctx.Writer.WriteLine ")"
    
    let writeModuleMembers (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.Write $"let Repeated{propertyName (containingType, field)}Codec = "
        writeCodecCode field ctx
        ctx.Writer.WriteLine ""

    let create (field: Field, containingType: Message option) =
        match containingType with
        | Some t ->
            {
                WriteMember = RepeatedPrimitiveFieldConverter.writeMember (field, t)
                WriteMemberInit = RepeatedPrimitiveFieldConverter.writeMemberInit (field, t)
                WriteOneOfCase = RepeatedPrimitiveFieldConverter.writeOneOfCase (field, t)
                WriteSerializedSizeCode = RepeatedPrimitiveFieldConverter.writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = RepeatedPrimitiveFieldConverter.writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = RepeatedPrimitiveFieldConverter.writeCloningCode (field, t)
                WriteMergingCode = RepeatedPrimitiveFieldConverter.writeMergingCode (field, t)
                WriteParsingCode = RepeatedPrimitiveFieldConverter.writeParsingCode (field, t)
                WriteOneOfParsingCode = RepeatedPrimitiveFieldConverter.writeOneOfParsingCode (field, t)
                WriteSerializationCode = RepeatedPrimitiveFieldConverter.writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = RepeatedPrimitiveFieldConverter.writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = writeModuleMembers (field, t)
                WriteCodecCode = writeCodecCode field
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field)
            }

module MessageFieldConverter =
    let writeSerializedSizeCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        let tagSize = tagSize field

        if field.Type = ValueSome FieldType.Group
        then ctx.Writer.Write $"{tagSize} + global.Google.Protobuf.CodedOutputStream.ComputeGroupSize({id})"
        else ctx.Writer.Write $"{tagSize} + global.Google.Protobuf.CodedOutputStream.ComputeMessageSize({id})"

    let writeSerializedSizeCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.Write $"if {propCheck} then size <- size + "
        writeSerializedSizeCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.WriteLine ""

    let writeCloningCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propName} = me.{propName} |> global.Microsoft.FSharp.Core.ValueOption.map (fun x -> (x :> global.Google.Protobuf.IMessage<{typeNameWithoutOption (ctx, field)}>).Clone())"

    let writeMergingCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        let propCheck = hasPropertyCheck (ctx, containingType, field, "other")
        ctx.Writer.WriteLines [
            $"if {propCheck}"
            $"then"
        ]

        ctx.Writer.Indent()
        ctx.Writer.WriteLines [
            $"if me.{propName}.IsNone"
            $"then me.{propName} <- ValueSome({typeNameWithoutOption (ctx, field)}.empty())"
            $"(me.{propName}.Value :> global.Google.Protobuf.IMessage<{typeNameWithoutOption (ctx, field)}>).MergeFrom(other.{propName}.Value)"
        ]
        ctx.Writer.Outdent()

    let writeParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeParsingCodeTemplate (ctx, field) <| fun () ->
            ctx.Writer.WriteLines [
                $"if me.{propertyName (containingType, field)}.IsNone"
                $"then me.{propertyName (containingType, field)} <- ValueSome({typeNameWithoutOption (ctx, field)}.empty())"
            ]

            if field.Type = ValueSome FieldType.Group
            then ctx.Writer.WriteLine $"input.ReadGroup(me.{propertyName (containingType, field)}.Value)"
            else ctx.Writer.WriteLine $"input.ReadMessage(me.{propertyName (containingType, field)}.Value)"

    let writeOneOfParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"let value = {typeNameWithoutOption (ctx, field)}.empty()"
        if field.Type = ValueSome FieldType.Group
        then ctx.Writer.WriteLine $"input.ReadGroup(value)"
        else ctx.Writer.WriteLine $"input.ReadMessage(value)"

    let writeSerializationCodeWithoutCheck (field: Field, containingType: Message, containerMessages: Message list) (ctx: FileContext) (id: string) =
        let tagBytes = Helpers.tagBytes field
        let tagBytesString = tagBytes |> Seq.map (sprintf "%iuy") |> String.concat ", "
        ctx.Writer.WriteLines [
            $"output.WriteRawTag({tagBytesString})"
            $"output.Write{capitalizedTypeName field}({id})"
        ]

        if field.Type = ValueSome FieldType.Group
        then
            let tagBytes = Helpers.rawTagBytes <| Helpers.groupEndTag (ctx, containingType, containerMessages)
            let tagBytesString = tagBytes |> Seq.map (sprintf "%iuy") |> String.concat ", "
            ctx.Writer.WriteLine $"output.WriteRawTag({tagBytesString})"

    let writeSerializationCode (field: Field, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        let propCheck = hasPropertyCheck (ctx, containingType, field, "me")
        ctx.Writer.WriteLines [
            $"if {propCheck}"
            "then"
        ]
        ctx.Writer.Indent()
        writeSerializationCodeWithoutCheck (field, containingType, containerMessages) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.Outdent()
    
    let writeCodecCode (field: Field, containingType: Message option, containerMessages: Message list) (ctx: FileContext) =
        match containingType with
        | Some(containingType) ->
            if field.Type = ValueSome FieldType.Group
            then
                let endTag = Helpers.groupEndTag (ctx, containingType, containerMessages)
                ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.ForMessage({Helpers.makeTag field}u, {endTag}u, {typeNameWithoutOption (ctx, field)}.Parser)"
            else ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.ForMessage({Helpers.makeTag field}u, {typeNameWithoutOption (ctx, field)}.Parser)"
        | None -> ctx.Writer.Write $"global.Google.Protobuf.FieldCodec.ForMessage({Helpers.makeTag field}u, {typeNameWithoutOption (ctx, field)}.Parser)"

    let writeExtensionCode (field: Field, containerMessages: Message list) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)

        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.Extension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "

        writeCodecCode (field, None, containerMessages) ctx
        ctx.Writer.WriteLine ")"

    let create (field: Field, containingType: Message option, containerMessages: Message list) =
        match containingType with
        | Some t ->
            {
                WriteMember = PrimitiveFieldConverter.writeMember (field, t)
                WriteMemberInit = PrimitiveFieldConverter.writeMemberInit (field, t)
                WriteOneOfCase = PrimitiveFieldConverter.writeOneOfCase (field, t)
                WriteSerializedSizeCode = writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = writeCloningCode (field, t)
                WriteMergingCode = writeMergingCode (field, t)
                WriteParsingCode = writeParsingCode (field, t)
                WriteOneOfParsingCode = writeOneOfParsingCode (field, t)
                WriteSerializationCode = writeSerializationCode (field, t, containerMessages)
                WriteSerializationCodeWithoutCheck = writeSerializationCodeWithoutCheck (field, t, containerMessages)
                WriteModuleMembers = ignore
                WriteCodecCode = writeCodecCode (field, Some(t), containerMessages)
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field, [])
            }

module RepeatedMessageFieldConverter =
    let writeExtensionCode (field: Field, containerMessages: Message list) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)

        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.RepeatedExtension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, "

        MessageFieldConverter.writeCodecCode (field, None, containerMessages) ctx
        ctx.Writer.WriteLine ")"

    let writeModuleMembers (field: Field, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        ctx.Writer.Write $"let Repeated{propertyName (containingType, field)}Codec = "
        MessageFieldConverter.writeCodecCode (field, Some(containingType), containerMessages) ctx
        ctx.Writer.WriteLine ""

    let create (field: Field, containingType: Message option, containerMessages: Message list) =
        match containingType with
        | Some t ->
            {
                WriteMember = RepeatedPrimitiveFieldConverter.writeMember (field, t)
                WriteMemberInit = RepeatedPrimitiveFieldConverter.writeMemberInit (field, t)
                WriteOneOfCase = RepeatedPrimitiveFieldConverter.writeOneOfCase (field, t)
                WriteSerializedSizeCode = RepeatedPrimitiveFieldConverter.writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = RepeatedPrimitiveFieldConverter.writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = RepeatedPrimitiveFieldConverter.writeCloningCode (field, t)
                WriteMergingCode = RepeatedPrimitiveFieldConverter.writeMergingCode (field, t)
                WriteParsingCode = RepeatedPrimitiveFieldConverter.writeParsingCode (field, t)
                WriteOneOfParsingCode = RepeatedPrimitiveFieldConverter.writeOneOfParsingCode (field, t)
                WriteSerializationCode = RepeatedPrimitiveFieldConverter.writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = RepeatedPrimitiveFieldConverter.writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = writeModuleMembers (field, t, containerMessages)
                WriteCodecCode = MessageFieldConverter.writeCodecCode (field, Some(t), containerMessages)
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field, [])
            }

module MapFieldConverter =
    let getMapFields (ctx: FileContext, field: Field) =
        let t = Helpers.findMessageType ctx field.TypeName.Value
        let keyField =
            t.Message.Field
            |> Seq.filter (fun f -> f.Name = ValueSome "key")
            |> Seq.exactlyOne
        let valueField =
            t.Message.Field
            |> Seq.filter (fun f -> f.Name = ValueSome "value")
            |> Seq.exactlyOne
        (t, keyField, valueField)

    let mapTypeName (ctx: FileContext, field: Field) =
        let _, keyField, valueField = getMapFields (ctx, field)
        $"global.Google.Protobuf.Collections.MapField<{typeNameWithoutOption (ctx, keyField)}, {typeNameWithoutOption (ctx, valueField)}>"

    let writeMember (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{propertyName (containingType, field)}: {mapTypeName (ctx, field)}"

    let writeMemberInit (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propertyName (containingType, field)} = {mapTypeName (ctx, field)}()"

    let writeOneOfCase (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"| {propertyName (containingType, field)} of {mapTypeName (ctx, field)}>"

    let writeSerializedSizeCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        ctx.Writer.Write $"{id}.CalculateSize({Helpers.messageTypeName containingType}.Map{propertyName (containingType, field)}Codec)"

    let writeSerializedSizeCode (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.Write "size <- size + "
        writeSerializedSizeCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"
        ctx.Writer.WriteLine ""

    let writeCloningCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propName} = me.{propName}.Clone()"

    let writeMergingCode (field: Field, containingType: Message) (ctx: FileContext) =
        let propName = propertyName (containingType, field)
        ctx.Writer.WriteLine $"me.{propName}.Add(other.{propName})"

    let writeParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeParsingCodeTemplate (ctx, field) <| fun () ->
            ctx.Writer.WriteLine $"me.{propertyName (containingType, field)}.AddEntriesFrom(&input, {Helpers.messageTypeName containingType}.Map{propertyName (containingType, field)}Codec)"

    let writeOneOfParsingCode (field: Field, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLines [
            $"let value = {mapTypeName (ctx, field)}()"
            $"value.AddEntriesFrom(&input, {Helpers.messageTypeName containingType}.Map{propertyName (containingType, field)}Codec)"
        ]

    let writeSerializationCodeWithoutCheck (field: Field, containingType: Message) (ctx: FileContext) (id: string) =
        ctx.Writer.WriteLine $"{id}.WriteTo(&output, {Helpers.messageTypeName containingType}.Map{propertyName (containingType, field)}Codec)"

    let writeSerializationCode (field: Field, containingType: Message) (ctx: FileContext) =
        writeSerializationCodeWithoutCheck (field, containingType) ctx $"me.{propertyAccess (ctx, containingType, field)}"

    let writeExtensionCode (field: Field) (ctx: FileContext) =
        addDeprecatedFlag (ctx, field)
        ctx.Writer.Write
            $"let {Helpers.snakeToCamelCase (Helpers.pascalToCamelCase field.Name.Value)} = global.Google.Protobuf.RepeatedExtension<{Helpers.getExtendee field},{typeNameWithoutOption (ctx, field)}>({field.Number}, global.Google.Protobuf.FieldCodec.For{capitalizedTypeName field}({Helpers.makeTag field}u))"

    let writeCodecCode (field: Field, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        let mapEntryType, keyField, valueField = getMapFields (ctx, field)
        let keyConv = SingleFieldConverterFactory.createWriter (keyField, ctx, Some mapEntryType.Message, containerMessages @ [ containingType ])
        let valueConv = SingleFieldConverterFactory.createWriter (valueField, ctx, Some mapEntryType.Message, containerMessages @ [ containingType ])
        
        ctx.Writer.Write $"{mapTypeName (ctx, field)}.Codec("
        keyConv.WriteCodecCode (ctx)
        ctx.Writer.Write $", "
        valueConv.WriteCodecCode (ctx)
        ctx.Writer.Write $", {Helpers.makeTag field}u)"
    
    let writeModuleMembers (field: Field, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        ctx.Writer.Write $"let Map{propertyName (containingType, field)}Codec = "
        writeCodecCode (field, containingType, containerMessages) ctx
        ctx.Writer.WriteLine ""

    let create (field: Field, containingType: Message option, containerMessages: Message list) =
        match containingType with
        | Some t ->
            {
                WriteMember = writeMember (field, t)
                WriteMemberInit = writeMemberInit (field, t)
                WriteOneOfCase = writeOneOfCase (field, t)
                WriteSerializedSizeCode = writeSerializedSizeCode (field, t)
                WriteSerializedSizeCodeWithoutCheck = writeSerializedSizeCodeWithoutCheck (field, t)
                WriteCloningCode = writeCloningCode (field, t)
                WriteMergingCode = writeMergingCode (field, t)
                WriteParsingCode = writeParsingCode (field, t)
                WriteOneOfParsingCode = writeOneOfParsingCode (field, t)
                WriteSerializationCode = writeSerializationCode (field, t)
                WriteSerializationCodeWithoutCheck = writeSerializationCodeWithoutCheck (field, t)
                WriteModuleMembers = writeModuleMembers (field, t, containerMessages)
                WriteCodecCode = writeCodecCode (field, t, containerMessages)
                WriteExtensionCode = fun _ -> raise <| System.NotImplementedException()
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = writeExtensionCode (field)
            }

module SingleFieldConverterFactory =
    let createWriter (field: Field, ctx: FileContext, containingType: Message option, containerMessages: Message list) : FieldConverter.FieldWriter =
        match field.Type.Value with
        | FieldType.Group
        | FieldType.Message ->
            if field.Label = ValueSome FieldLabel.Repeated
            then
                if Helpers.isMapEntryField (ctx, field)
                then MapFieldConverter.create (field, containingType, containerMessages)
                else RepeatedMessageFieldConverter.create (field, containingType, containerMessages)
            else MessageFieldConverter.create (field, containingType, containerMessages)

        | FieldType.Enum ->
            if field.Label = ValueSome FieldLabel.Repeated
            then RepeatedEnumFieldConverter.create (field, containingType)
            else EnumFieldConverter.create (field, containingType)

        | _ ->
            if field.Label = ValueSome FieldLabel.Repeated
            then RepeatedPrimitiveFieldConverter.create (field, containingType)
            else PrimitiveFieldConverter.create (field, containingType)

module OneOfFieldConverter =
    let writeMember (oneOf: OneOf, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        ctx.Writer.WriteLine $"mutable {oneOfPropertyName oneOf}: {oneOfTypeName (oneOf, containingType, containerMessages, ctx.File)}"

    let writeMemberInit (oneOf: OneOf, containingType: Message) (ctx: FileContext) =
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{oneOfPropertyName oneOf} = ValueNone"

    let writeSerializedSizeCode (oneOf: OneOf, fields: Field list, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        ctx.Writer.WriteLines [
            $"match me.{oneOfPropertyName oneOf} with"
            "| ValueNone -> ()"
        ]

        for f in fields do
            ctx.Writer.Write $"| ValueSome ({oneOfCaseName (oneOf, f, containingType, containerMessages, ctx.File)} x) -> size <- size + "
            let conv = SingleFieldConverterFactory.createWriter (f, ctx, Some containingType, containerMessages)
            conv.WriteSerializedSizeCodeWithoutCheck ctx "x"
            ctx.Writer.WriteLine ""

    let writeCloningCode (oneOf: OneOf, containingType: Message) (ctx: FileContext) =
        let propName = oneOfPropertyName oneOf
        ctx.Writer.WriteLine $"{Helpers.messageTypeName containingType}.{propName} = me.{propName}"

    let writeMergingCode (oneOf: OneOf) (ctx: FileContext) =
        let propName = oneOfPropertyName oneOf
        ctx.Writer.WriteLines [
            $"if other.{propName} <> ValueNone"
            $"then me.{propName} <- other.{propName}"
        ]

    let writeParsingCode (oneOf: OneOf, fields: Field list, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        for f in fields do
            writeParsingCodeTemplate (ctx, f) <| fun () ->
                let conv = SingleFieldConverterFactory.createWriter (f, ctx, Some containingType, containerMessages)
                conv.WriteOneOfParsingCode ctx
                ctx.Writer.WriteLine $"me.{oneOfPropertyName oneOf} <- ValueSome({oneOfCaseName (oneOf, f, containingType, containerMessages, ctx.File)}(value))"

    let writeSerializationCode (oneOf: OneOf, fields: Field list, containingType: Message, containerMessages: Message list) (ctx: FileContext) =
        ctx.Writer.WriteLines [
            $"match me.{oneOfPropertyName oneOf} with"
            "| ValueNone -> ()"
        ]

        for f in fields do
            ctx.Writer.WriteLine $"| ValueSome ({oneOfCaseName (oneOf, f, containingType, containerMessages, ctx.File)} x) ->"
            ctx.Writer.Indent()
            let conv = SingleFieldConverterFactory.createWriter (f, ctx, Some containingType, containerMessages)
            conv.WriteSerializationCodeWithoutCheck ctx "x"
            ctx.Writer.Outdent()

    let create (oneOf: OneOf, fields: Field list, containingType: Message option, containerMessages: Message list) =
        match containingType with
        | Some t ->
            {
                WriteMember = writeMember (oneOf, t, containerMessages)
                WriteMemberInit = writeMemberInit (oneOf, t)
                WriteOneOfCase = fun _ -> invalidOp "Nested one of not supported"
                WriteSerializedSizeCode = writeSerializedSizeCode (oneOf, fields, t, containerMessages)
                WriteSerializedSizeCodeWithoutCheck = fun _ _ -> invalidOp "Not supported for one of fields"
                WriteCloningCode = writeCloningCode (oneOf, t)
                WriteMergingCode = writeMergingCode oneOf
                WriteParsingCode = writeParsingCode (oneOf, fields, t, containerMessages)
                WriteOneOfParsingCode = fun _ -> invalidOp "Not supported for one of fields"
                WriteSerializationCode = writeSerializationCode (oneOf, fields, t, containerMessages)
                WriteSerializationCodeWithoutCheck = fun _ _ -> invalidOp "Not supported for one of fields"
                WriteModuleMembers = ignore
                WriteCodecCode = fun _ -> invalidOp "One of codec not supported"
                WriteExtensionCode = fun _ -> invalidOp "One of extension not supported"
            }
        | None ->
            { NotImplementedWriter with
                WriteExtensionCode = fun _ -> invalidOp "One of extension not supported"
            }

module FieldConverterFactory =
    let createWriter (field: FSField, ctx: FileContext, containingType: Message option, containerMessages: Message list) : FieldConverter.FieldWriter =
        match field with
        | Single field -> SingleFieldConverterFactory.createWriter (field, ctx, containingType, containerMessages)
        | OneOf (oneOf, fields) -> OneOfFieldConverter.create (oneOf, fields, containingType, containerMessages)
