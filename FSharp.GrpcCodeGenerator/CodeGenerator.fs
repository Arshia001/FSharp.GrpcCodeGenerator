module CodeGenerator

open System
open Google.Protobuf.FSharp

let private generateFile (file: File, dependencies: File list, options: Options) : Compiler.CodeGeneratorResponse.Types.File =
    if Helpers.isProto2 file && not <| Helpers.isBuiltInGoogleDefinition file
    then failwith "proto2 not supported"

    let writer = FSharpFileWriter.create ()
    let ctx = {
        Writer = writer
        File = file
        Dependencies = dependencies
        Options = options
    }
    FileConverter.generateCode ctx
    { Compiler.CodeGeneratorResponse.Types.File.empty() with
        Name = ValueSome <| Helpers.outputFileName file
        Content = ValueSome <| writer.GetText()
    }

let private findFile (req: Compiler.CodeGeneratorRequest) name =
    req.ProtoFile
    |> Seq.filter (fun f -> f.Name = ValueSome name)
    |> Seq.exactlyOne

let parseOptions (opts: string voption) =
    let defaultOptions = {
        InternalAccess = false
        ServerServices = true
        ClientServices = true
    }

    match opts with
    | ValueNone -> defaultOptions
    | ValueSome opts ->
        opts.Split ','
        |> Seq.map (fun s ->
            let split = s.Split([| '=' |], 2)
            match split.Length with
            | 2 -> split.[0].ToLower(), split.[1]
            | _ -> s.ToLower(), ""
        )
        |> Seq.fold (fun opts (key, value) ->
            match key with
            | "internal_access" -> { opts with InternalAccess = true }
            | "no_server" -> { opts with ServerServices = false }
            | "no_client" -> { opts with ClientServices = false }
            | _ -> failwithf "Unknown parameter %s" key
        ) defaultOptions

let generate (req: Compiler.CodeGeneratorRequest) : Compiler.CodeGeneratorResponse =
    try
        let options = parseOptions req.Parameter
        let files =
            req.FileToGenerate
            |> Seq.map (fun name ->
                let file = findFile req name
                let deps =
                    file.Dependency
                    |> Seq.map (findFile req)
                    |> Seq.toList
                generateFile (file, deps, options)
            )

        let r = { Compiler.CodeGeneratorResponse.empty() with SupportedFeatures = ValueSome <| uint64 Compiler.CodeGeneratorResponse.Types.Feature.Proto3Optional }
        r.File.AddRange files
        r
#if DEBUG
    with ex -> { Compiler.CodeGeneratorResponse.empty() with Error = ValueSome (string ex) }
#else
    with ex -> { Compiler.CodeGeneratorResponse.empty() with Error = ValueSome ex.Message }
#endif
