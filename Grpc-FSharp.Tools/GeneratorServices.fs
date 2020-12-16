module Grpc.FSharp.Tools.GeneratorServices

open System
open Microsoft.Build.Framework
open Microsoft.Build.Utilities

let private dotSlash = "." + string IO.Path.DirectorySeparatorChar

let endWithSlash s =
    if s = ""
    then dotSlash
    elif s.EndsWith "/" || s.EndsWith "\\"
    then s
    else s + string IO.Path.DirectorySeparatorChar

let relativeDir (root: string, proto: string, log: TaskLoggingHelper) =
    let protoDir = IO.Path.GetDirectoryName proto
    let rootDir = endWithSlash root |> IO.Path.GetDirectoryName |> endWithSlash
    
    if rootDir = dotSlash
    then protoDir
    else

    let protoDir, rootDir =
        if Platform.isFSCaseInsensitive
        then protoDir.ToLowerInvariant(), rootDir.ToLowerInvariant()
        else protoDir, rootDir
    let protoDir = endWithSlash protoDir
    if not <| protoDir.StartsWith rootDir
    then
        log.LogWarning <| sprintf "Protobuf item %s has the ProtoRoot metadata %s which is \
                                   not prefix to its path. Cannot compute relative path." proto root
        ""
    else
        protoDir.Substring rootDir.Length

let patchOutputDirectory (protoItem: ITaskItem, log: TaskLoggingHelper) : ITaskItem =
    let result = TaskItem(protoItem)
    let root = result.GetMetadata Metadata.protoRoot
    let proto = result.ItemSpec
    let relative = relativeDir (root, proto, log)

    let outDir = result.GetMetadata Metadata.outputDir
    let pathStem = IO.Path.Combine(outDir, relative)
    result.SetMetadata(Metadata.outputDir, pathStem)

    upcast result

let private snakeToPascalCase (s: string) =
    let firstCharToUpper (s: string) =
        match s.Length with
        | 0 -> s
        | 1 -> s.ToUpper()
        | _ -> string (Char.ToUpper s.[0]) + s.[1..]
    
    s.Split '_' |> Seq.map firstCharToUpper |> String.concat ""

let outputFileName (protoItem: ITaskItem) =
    let proto = protoItem.ItemSpec
    let baseName = IO.Path.GetFileNameWithoutExtension proto
    let outDir = protoItem.GetMetadata Metadata.outputDir
    let fileName = snakeToPascalCase baseName
    IO.Path.Combine(outDir, fileName)
