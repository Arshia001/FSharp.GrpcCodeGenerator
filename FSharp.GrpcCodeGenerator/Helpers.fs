module Helpers

open System
open Google.Protobuf

let private firstCharToUpper (s: string) =
    match s.Length with
    | 0 -> s
    | 1 -> s.ToUpper()
    | _ -> string (Char.ToUpper s.[0]) + s.[1..]

let snakeToPascalCase includesDots (s: string) =
    let convertOne (s: string) =
        s.Split '_' |> Seq.map firstCharToUpper |> String.concat ""

    if includesDots
    then s.Split '.' |> Seq.map convertOne |> String.concat "."
    else convertOne s

let shoutyToPascalCase (s: string) =
    s.Split '_'
    |> Seq.map (fun s -> s.ToLower() |> firstCharToUpper)
    |> String.concat ""

let stripDotProto (s: string) =
    let i = s.LastIndexOf '.'
    if i >= 0 then s.Substring(0, i) else s

let tryRemovePrefix (prefix: string) (s: string) =
    let prefixExcludingUnderscores (s: string, l: int) =
        let chars =
            s
            |> Seq.indexed
            |> Seq.filter (fun (_, c) -> c <> '_')

        if Seq.length chars >= l
        then
            let chars = chars |> Seq.take l
            let prefix = String(chars |> Seq.map snd |> Seq.toArray)
            Some (prefix, Seq.last chars |> fst)
        else None

    let prefix = String(prefix |> Seq.filter ((<>) '_') |> Seq.toArray)
    match prefixExcludingUnderscores (s, prefix.Length) with
    | Some (prefix', i) ->
        if prefix.Equals(prefix', StringComparison.InvariantCultureIgnoreCase)
        then s.Substring(i + 1)
        else s
    | None -> s

let enumValueName (enumName, valueName) =
    let name =
        let withoutPrefix =
            tryRemovePrefix enumName valueName
            |> shoutyToPascalCase
        if withoutPrefix = ""
        then shoutyToPascalCase valueName
        else withoutPrefix
    if Char.IsDigit name.[0]
    then "_" + name
    else name

let fileNameBase (file: File) =
    let name = file.Name.Value
    let i =
        let i = name.LastIndexOf '/'
        if i < 0 then 0 else i + 1
    name.Substring i
    |> stripDotProto
    |> snakeToPascalCase false

let fileNamespace (file: File) =
    let ns =
        match file.Options with
        | ValueSome { CsharpNamespace = ValueSome ns } -> ns
        | _ ->
        match file.Package with
        | ValueSome s when s <> "" -> snakeToPascalCase true s
        // Namespaces are mandatory in F# source files, we always need to have one.
        // The file name is unlikely to be empty.
        | _ -> fileNameBase file
    if ns.StartsWith "Google.Protobuf."
    then "Google.Protobuf.FSharp." + ns.Substring ("Google.Protobuf.".Length)
    else ns

let messageTypeName (msg: Message) = msg.Name.Value

let reflectionClassUnqualifiedName file = fileNameBase file + "Reflection"

let extensionClassUnqualifiedName file = fileNameBase file + "Extensions"

let descriptorName (msg: Message, containingTypes: Message list) =
    let messageNames =
        Seq.append containingTypes [ msg ]
        |> Seq.map messageTypeName
        |> String.concat "_"
    messageNames + "_Descriptor"

let messageIndex (msg: Message, messages: Message seq) =
    messages
    |> Seq.indexed
    |> Seq.filter (fun (_, m) -> m = msg)
    |> Seq.head
    |> fst

let isProto2 (file: File) = file.Syntax = ValueSome "proto2"

let isBuiltInGoogleDefinition (file: File) = file.Package.Value.StartsWith "google.protobuf"

let qualifiedName (name: string, file: File) =
    let res =
        let ns = fileNamespace file
        if ns = ""
        then ""
        else ns + "."
    "global." + res + name

let innerTypeName (name: string, outer: string) = outer + ".Types." + name

let qualifiedInnerName (name: string, outer: string, file: File) =
    let res =
        let ns = fileNamespace file
        if ns = ""
        then ""
        else ns + "."
    if outer = ""
    then "global." + res + name
    else "global." + res + (outer + "." + name).Replace(".", ".Types.")

let qualifiedInnerNameFromMessages (name: string, containerMessages: Message list, file: File) =
    let fileNs =
        let ns = fileNamespace file
        if ns = ""
        then ""
        else ns + "."
    let messages = containerMessages |> Seq.map (fun s -> s.Name.Value + ".Types.") |> String.concat ""
    "global." + fileNs + messages + name

let reflectionClassName file =
    let ns =
        let ns = fileNamespace file
        if ns = ""
        then ""
        else ns + "."
    "global." + ns + reflectionClassUnqualifiedName file

let private fieldName (msg: Message, field: Field) =
    if field.Type = ValueSome FieldType.Group
    then msg.Name.Value
    else field.Name.Value

let propertyName (msg: Message) (desc: Field) = fieldName (msg, desc) |> snakeToPascalCase false

let fieldConstantName (msg: Message, desc: Field) = propertyName msg desc + "FieldNumber"

let fullExtensionName (file: File, field: Field) = // TODO: correct?
    if field.Type = ValueSome FieldType.Group
    then failwithf "Extension field of type Group not supported: %s" field.Name.Value

    match field.Extendee with
    | ValueSome e when e <> "" -> qualifiedName (e, file) + ".Extensions." + propertyName Unchecked.defaultof<_> field
    | _ -> extensionClassUnqualifiedName file + "." + propertyName Unchecked.defaultof<_> field

let outputFileName (file: File) = fileNameBase file + ".fs"

let wireTypeForFieldType (t: FieldType) =
    match t with
    | FieldType.Double -> WireFormat.WireType.Fixed64
    | FieldType.Float -> WireFormat.WireType.Fixed32
    | FieldType.Int64 -> WireFormat.WireType.Varint
    | FieldType.Uint64 -> WireFormat.WireType.Varint
    | FieldType.Int32 -> WireFormat.WireType.Varint
    | FieldType.Fixed64 -> WireFormat.WireType.Fixed64
    | FieldType.Fixed32 -> WireFormat.WireType.Fixed32
    | FieldType.Bool -> WireFormat.WireType.Varint
    | FieldType.String -> WireFormat.WireType.LengthDelimited
    | FieldType.Group -> WireFormat.WireType.StartGroup
    | FieldType.Message -> WireFormat.WireType.LengthDelimited
    | FieldType.Bytes -> WireFormat.WireType.LengthDelimited
    | FieldType.Uint32 -> WireFormat.WireType.Varint
    | FieldType.Enum -> WireFormat.WireType.Varint
    | FieldType.Sfixed32 -> WireFormat.WireType.Fixed32
    | FieldType.Sfixed64 -> WireFormat.WireType.Fixed64
    | FieldType.Sint32 -> WireFormat.WireType.Varint
    | FieldType.Sint64 -> WireFormat.WireType.Varint
    | _ -> failwithf "Unknown field type %A" t

let makeTag (field: Field) = Google.Protobuf.WireFormat.MakeTag(field.Number.Value, wireTypeForFieldType field.Type.Value)

let makeTagWithType (field: Field, wireType: Google.Protobuf.WireFormat.WireType) =
    Google.Protobuf.WireFormat.MakeTag(field.Number.Value, wireType)

// I don't know of a method which will return this information directly
let rawTagBytes (tag: uint32) =
    use stream = new IO.MemoryStream()
    use cos = new CodedOutputStream(stream)
    cos.WriteTag(tag)
    cos.Flush()
    stream.ToArray()

let tagBytes (field: Field) =
    makeTag field |> rawTagBytes

let fixedSize (type': FieldType) =
    match type' with
    | FieldType.Fixed32 -> Some 4
    | FieldType.Fixed64 -> Some 8
    | FieldType.Sfixed32 -> Some 4
    | FieldType.Sfixed64 -> Some 8
    | FieldType.Float -> Some 4
    | FieldType.Double -> Some 8
    | FieldType.Bool -> Some 1
    | _ -> None

let isFieldPackable (f: Field) =
    let wireType = wireTypeForFieldType f.Type.Value
    f.Label = ValueSome FieldLabel.Repeated &&
    (
        wireType = WireFormat.WireType.Varint ||
        wireType = WireFormat.WireType.Fixed32 ||
        wireType = WireFormat.WireType.Fixed64
    )

let fileDescriptorProtoToBase64 (file: File) =
    use stream = new IO.MemoryStream()
    let file = file.Clone()
    file.SourceCodeInfo <- ValueNone
    (file :> IMessage).WriteTo stream
    Convert.ToBase64String <| stream.ToArray()

let flatMapFileTypes fFile fMessage (file: File) =
    let rec subTypes (ns: string, containerTypes) (msg: Message) =
        msg.NestedType
        |> Seq.map (subTypes (ns + "." + msg.Name.Value, msg :: containerTypes))
        |> Seq.concat
        |> Seq.append [ fMessage (msg, containerTypes, ns) ]

    let fileNs = "." + file.Package.Value
    file.MessageType
    |> Seq.map (subTypes (fileNs, []))
    |> Seq.concat
    |> Seq.append [ fFile (file, fileNs) ]

let flatMapAllFiles fFile fMessage (ctx: FileContext) =
    [ ctx.File ]
    |> Seq.append ctx.Dependencies
    |> Seq.map (fun file -> flatMapFileTypes fFile (fMessage file) file)
    |> Seq.concat

type FoundMessageInfo = {
    Message: Message
    ContainerMessages: Message list
    File: File
}

let findMessageType (ctx: FileContext) (typeName: string) =
    flatMapAllFiles
        (fun _ -> [])
        (fun f (msg, container, ns) -> [ { Message = msg; ContainerMessages = List.rev container; File = f}, ns + "." + msg.Name.Value ])
        ctx
    |> Seq.concat
    |> Seq.filter (fun (_, n) -> n = typeName)
    |> Seq.map fst
    |> Seq.tryHead
    |> Option.defaultWith (fun () -> failwithf "Type not found in proto files: %s" typeName)

type FoundEnumInfo = {
    Enum: ProtoEnum
    ContainerMessages: Message list
    File: File
}

let findEnumType (ctx: FileContext) (typeName: string) =
    let foundEnumInfos (e: ProtoEnum seq, f, container, ns) =
        e |> Seq.map (fun e -> { Enum = e; File = f; ContainerMessages = List.rev container}, ns + "." + e.Name.Value)

    flatMapAllFiles
        (fun (f, ns) -> foundEnumInfos (f.EnumType, f, [], ns))
        (fun f (msg, container, ns) -> foundEnumInfos (msg.EnumType, f, msg :: container, ns + "." + msg.Name.Value))
        ctx
    |> Seq.concat
    |> Seq.filter (fun (_, n) -> n = typeName)
    |> Seq.map fst
    |> Seq.tryHead
    |> Option.defaultWith (fun () -> failwithf "Type not found in proto files: %s" typeName)

let isMapEntryMessage (msg: Message) = match msg.Options with ValueSome { MapEntry = ValueSome true } -> true | _ -> false

let isMapEntryField (ctx: FileContext, field: Field) =
    if field.Type = ValueSome FieldType.Message
    then isMapEntryMessage <| (findMessageType ctx field.TypeName.Value).Message
    else false

let groupEndTag (ctx: FileContext, message: Message, containerMessages: Message list) =
    let isStartOfGroup (f: Field) = f.Type = ValueSome FieldType.Group && (findMessageType ctx f.TypeName.Value).Message = message
    let makeGroupEndTag (f: Field) = makeTagWithType (f, Google.Protobuf.WireFormat.WireType.EndGroup)
    let findGroupEndTag = Seq.filter isStartOfGroup >> Seq.map makeGroupEndTag >> Seq.tryHead
    match containerMessages with
    | [] ->
        findGroupEndTag ctx.File.Extension
        |> Option.defaultValue 0u
    | _ ->
        let containingType = List.last containerMessages
        findGroupEndTag containingType.Field
        |> Option.defaultWith (fun () ->
            findGroupEndTag containingType.Extension
            |> Option.defaultValue 0u
        )

let makeOptionType (typeName: string) =
    if typeName.StartsWith "ValueOption<"
    then typeName
    else $"ValueOption<{typeName}>"

let containingOneOf (msg: Message, field: Field) =
    match field.OneofIndex with
    | ValueSome x when x >= 0 && x < msg.OneofDecl.Count -> ValueSome msg.OneofDecl.[x]
    | _ -> ValueNone

let oneOfFields (msg: Message, oneOf: OneOf) =
    let index =
        msg.OneofDecl
        |> Seq.indexed
        |> Seq.filter (fun (_, o) -> o = oneOf)
        |> Seq.head
        |> fst
    msg.Field
    |> Seq.filter (fun f -> f.OneofIndex = ValueSome index)
    |> Seq.toList

let writeGeneratedCodeAttribute (ctx: FileContext) =
    ctx.Writer.WriteLine "[<global.System.Diagnostics.DebuggerNonUserCodeAttribute>]"

let getStringBytes (s: string) =
    use res = new IO.MemoryStream()
    let mutable i = 0
    while i < s.Length do
        if
            i < s.Length - 4 &&
            s.[i] = '\\' &&
            Char.IsDigit(s.[i + 1]) &&
            Char.IsDigit(s.[i + 2]) &&
            Char.IsDigit(s.[i + 3])
        then
            res.WriteByte <| byte (
                64 * (int s.[i + 1] - int '0') +
                8 * (int s.[i + 2] - int '0') +
                (int s.[i + 3] - int '0')
            )
            i <- i + 4
        else
            res.WriteByte <| byte s.[i]
            i <- i + 1
    res.ToArray()
