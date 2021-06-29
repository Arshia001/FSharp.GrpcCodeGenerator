﻿module FileConverter

open System
open Google.Protobuf
open System.IO

let writeIntroduction (ctx: FileContext) =
    ctx.Writer.WriteLines [
        "// <auto-generated>"
        "//     Generated by the F# GRPC code generator. DO NOT EDIT!"
        $"//     source: {ctx.File.Name.Value}"
        "// </auto-generated>"
        $"namespace rec {Helpers.fileNamespace ctx.File}"
        $"#nowarn \"40\""
    ]

let rec writeGeneratedCodeInfo (ctx: FileContext, outerTypeName: string, msg: Message) =
    match msg.Options with
    | ValueSome { MapEntry = ValueSome true } ->
        ctx.Writer.Write "null"
    | _ ->

    let typeName = Helpers.qualifiedInnerName(msg.Name.Value, outerTypeName, ctx.File)
    ctx.Writer.Write $"new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo(typeof<{typeName}>, {typeName}.Parser, "

    if msg.Field.Count > 0
    then
        ctx.Writer.WriteAll [
            "[| "
            (
                msg.Field
                |> Seq.map (Helpers.propertyName msg >> sprintf "\"%s\"")
                |> String.concat "; "
            )
            " |], "
        ]
    else
        ctx.Writer.Write "null, "

    if msg.OneofDecl.Count > 0
    then
        ctx.Writer.WriteAll [
            "[| "
            (
                msg.OneofDecl
                |> Seq.map (fun o -> o.Name.Value |> Helpers.snakeToPascalCase false |> sprintf "\"%s\"")
                |> String.concat "; "
            )
            " |], "
        ]
    else
        ctx.Writer.Write "null, "

    if msg.EnumType.Count > 0
    then
        ctx.Writer.WriteAll [
            "[| "
            (
                msg.EnumType
                |> Seq.map (fun e -> $"typeof<{Helpers.innerTypeName(e.Name.Value, typeName)}>")
                |> String.concat "; "
            )
            " |], "
        ]
    else
        ctx.Writer.Write "null, "

    if msg.Extension.Count > 0
    then
        ctx.Writer.WriteAll [
            "[| "
            (
                msg.Extension
                |> Seq.map (fun e -> Helpers.fullExtensionName(ctx.File, e))
                |> String.concat "; "
            )
            " |], "
        ]
    else
        ctx.Writer.Write "null, "

    if msg.NestedType.Count > 0
    then
        ctx.Writer.Write "[| "

        for i in 0..msg.NestedType.Count - 1 do
            let outer =
                if outerTypeName = ""
                then msg.Name.Value
                else outerTypeName + "." + msg.Name.Value
            writeGeneratedCodeInfo (ctx, outer, msg.NestedType.[i])
            if i < msg.NestedType.Count - 1 then ctx.Writer.Write "; "
        
        ctx.Writer.Write " |]"
    else
        ctx.Writer.Write "null"

    ctx.Writer.Write ")"

let writeMessageDescriptors (ctx: FileContext) =
    let rec helper (ctx: MessageConverter.MessageContext) =
        let descriptorAccessor =
            match ctx.ContainerMessages with
            | [] -> $"Descriptor().MessageTypes.[{Helpers.messageIndex (ctx.Message, ctx.File.File.MessageType)}]"
            | _ ->
                let containingType = List.last ctx.ContainerMessages
                $"{Helpers.descriptorName (containingType, List.init' ctx.ContainerMessages)}().NestedTypes.[{Helpers.messageIndex (ctx.Message, containingType.NestedType)}]"

        Helpers.writeGeneratedCodeAttribute ctx.File
        ctx.File.Writer.WriteLine $"let internal {Helpers.descriptorName (ctx.Message, ctx.ContainerMessages)}() = {descriptorAccessor}"
        
        let containers = ctx.ContainerMessages @ [ ctx.Message ]
        for msg in ctx.Message.NestedType do
            helper
                { ctx with
                    Message = msg
                    ContainerMessages = containers
                }

    for msg in ctx.File.MessageType do
        helper {
            MessageConverter.File = ctx
            MessageConverter.Message = msg
            MessageConverter.ContainerMessages = []
            MessageConverter.OrderedFSFields = [] // not needed
        }

let writeReflectionDescriptor (ctx: FileContext) =
    ctx.Writer.WriteLine $"module {Helpers.accessSpecifier ctx}{Helpers.reflectionClassUnqualifiedName ctx.File} ="

    ctx.Writer.Indent()

    writeMessageDescriptors ctx

    ctx.Writer.WriteLine "let private descriptorBackingField: global.System.Lazy<global.Google.Protobuf.Reflection.FileDescriptor> ="

    ctx.Writer.Indent()
    ctx.Writer.WriteLine $"let descriptorData = global.System.Convert.FromBase64String(\"\\"
    ctx.Writer.Indent()
    let descLines =
        Helpers.fileDescriptorProtoToBase64 ctx.File
        |> Seq.chunkBySize 80
        |> Seq.map (fun c -> String(c))
    let lineCount = Seq.length descLines
    descLines
    |> Seq.indexed
    |> Seq.map (fun (i, s) -> if i = lineCount - 1 then $"{s}\")" else $"{s}\\")
    |> ctx.Writer.WriteLines
    ctx.Writer.Outdent()

    ctx.Writer.WriteLine "global.System.Lazy<_>("
    ctx.Writer.Indent()
    ctx.Writer.WriteLine "(fun () ->"
    ctx.Writer.Indent()
    
    ctx.Writer.WriteLine "global.Google.Protobuf.Reflection.FileDescriptor.FromGeneratedCode("
    ctx.Writer.Indent()
    ctx.Writer.WriteLine "descriptorData,"
    ctx.Writer.WriteLine "[|"
    ctx.Writer.Indent()
    
    ctx.Dependencies
    |> Seq.map (fun d -> Helpers.reflectionClassName d + ".Descriptor()")
    |> ctx.Writer.WriteLines

    ctx.Writer.Outdent()
    ctx.Writer.WriteLines [
        "|],"
        "new global.Google.Protobuf.Reflection.GeneratedClrTypeInfo("
    ]

    ctx.Writer.Indent()

    if ctx.File.EnumType.Count > 0
    then
        ctx.Writer.WriteLine "[|"
        ctx.Writer.Indent()

        ctx.File.EnumType
        |> Seq.map (fun e -> $"typeof<{Helpers.qualifiedName(e.Name.Value, ctx.File)}>")
        |> ctx.Writer.WriteLines

        ctx.Writer.Outdent()
        ctx.Writer.WriteLine "|],"
    else
        ctx.Writer.WriteLine "null,"

    if ctx.File.Extension.Count > 0
    then
        ctx.Writer.WriteLine "[|"
        ctx.Writer.Indent()

        ctx.File.Extension
        |> Seq.map (fun e -> Helpers.fullExtensionName (ctx.File, e))
        |> ctx.Writer.WriteLines

        ctx.Writer.Outdent()
        ctx.Writer.WriteLine "|],"
    else
        ctx.Writer.WriteLine "null,"

    if ctx.File.MessageType.Count > 0
    then
        ctx.Writer.WriteLine "[|"
        ctx.Writer.Indent()

        ctx.File.MessageType
        |> Seq.iter (fun m ->
            writeGeneratedCodeInfo (ctx, "", m)
            ctx.Writer.WriteLine ""
        )

        ctx.Writer.Outdent()
        ctx.Writer.WriteLine "|]"
    else
        ctx.Writer.WriteLine "null"

    ctx.Writer.Outdent()
    ctx.Writer.WriteLine ")"

    ctx.Writer.Outdent()
    ctx.Writer.WriteLine ")"
    
    ctx.Writer.Outdent()
    ctx.Writer.WriteLines [
        "),"
        "true"
    ]
    
    ctx.Writer.Outdent()
    ctx.Writer.WriteLine ")"

    ctx.Writer.Outdent()
    ctx.Writer.WriteLine "let Descriptor(): global.Google.Protobuf.Reflection.FileDescriptor = descriptorBackingField.Value"
    ctx.Writer.Outdent()

let generateCode (ctx: FileContext) =
    writeIntroduction ctx
    writeReflectionDescriptor ctx

    if ctx.File.Extension.Count > 0
    then
        ctx.Writer.WriteLine $"module {Helpers.accessSpecifier ctx}{Helpers.extensionClassUnqualifiedName ctx.File} ="

        ctx.Writer.Indent()
// let http: global.Google.Protobuf.Extension<global.Google.Protobuf.Reflection.MethodOptions, global.Google.Api.HttpRule> =
//     global.Google.Protobuf.Extension<global.Google.Protobuf.Reflection.MethodOptions, global.Google.Api.HttpRule>(72295728, global.Google.Protobuf.FieldCodec.ForMessage(578365826u, global.Google.Api.HttpRule.Parser))

        // ctx.File.Extension
        // |> Seq.iter (fun f ->
        //     use sw = new StreamWriter(Path.GetTempFileName())
        //     sw.WriteLine(f.Extendee.Value)
        //     sw.WriteLine(f.JsonName.Value)
        //     sw.WriteLine(f.Label.Value)
        //     sw.WriteLine(f.Name.Value)
        //     sw.WriteLine(f.Number.Value)
        //     // sw.WriteLine(f.TypeName.Value)
        //     sw.WriteLine(f.Type.Value)


        //     let fieldName = f.Name.Value
        //     let fieldNumber = f.Number.Value
        //     let extendee =
        //         if f.Extendee.Value.StartsWith(".google.protobuf") then
        //             f.Extendee.Value.Replace(".google.protobuf", "global.Google.Protobuf.Reflection")
        //         else
        //             f.Extendee.Value

        //     let typeName =
        //         match f.TypeName with
        //         | ValueSome(t) -> 
        //             if t.StartsWith(".") then
        //                 "global" + ((t).Split('.') |> Array.map (fun s -> Helpers.firstCharToUpper s) |> String.concat ".")
        //             else
        //                 t
        //         | ValueNone ->
        //             f.Type.Value.ToString()
                
        //     let tag = Helpers.makeTag f

    
        //     ctx.Writer.WriteLine $"let {Helpers.pascalToCamelCase fieldName}: global.Google.Protobuf.Extension<{extendee}, {typeName}> ="
        //     ctx.Writer.Indent()
        //     ctx.Writer.WriteLine $"global.Google.Protobuf.Extension<{extendee}, {typeName}>({fieldNumber}, global.Google.Protobuf.FieldCodec.ForMessage({tag}u, {typeName}.Parser))"
        //     ctx.Writer.Outdent()
        // )

        // TODO: Do we even need to support extensions? AFAIK, it's a proto2 concept
        ctx.File.Extension
        |> Seq.iter (fun f ->
           let conv = SingleFieldConverterFactory.createWriter (f, ctx, None, [])
           conv.WriteExtensionCode ctx
        )

        ctx.Writer.Outdent()

    if ctx.File.EnumType.Count > 0
    then
        ctx.File.EnumType
        |> Seq.iter (EnumConverter.writeEnum ctx)

    if ctx.File.MessageType.Count > 0
    then
        ctx.File.MessageType
        |> Seq.iter (fun msg -> MessageConverter.writeMessage (ctx, [], msg))

    if ctx.File.Service.Count > 0
    then
        ctx.File.Service
        |> Seq.iter (fun svc -> ServiceConverter.writeService (ctx, svc))

    ctx.Writer.WriteLine "// End of auto-generated code"
