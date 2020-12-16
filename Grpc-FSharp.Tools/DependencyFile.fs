module Grpc.FSharp.Tools.DependencyFile

open System
open Microsoft.Build.Framework
open Microsoft.Build.Utilities

let hashString64Hex (str: string) =
    use sha1 = Security.Cryptography.SHA1.Create()
    let hash = sha1.ComputeHash(Text.Encoding.UTF8.GetBytes str)
    
    hash
    |> Seq.take 8
    |> Seq.map (sprintf "%02x")
    |> String.concat ""

let directoryHash (proto: string) =
    let dirName = IO.Path.GetDirectoryName proto

    if Platform.isFSCaseInsensitive
    then dirName.ToLowerInvariant()
    else dirName
    |> hashString64Hex

let outputDirWithHash (outputDir: string, proto: string) =
    IO.Path.Combine (outputDir, directoryHash proto)

let depFileNameForProto (protoDepDir: string, proto: string) =
    let dirHash = directoryHash proto
    let fileName = IO.Path.GetFileNameWithoutExtension proto
    IO.Path.Combine (protoDepDir, sprintf "%s_%s.protodep" dirHash fileName)

let readDepFileLines (fileName: string, required: bool, log: TaskLoggingHelper) =
    try
        let result = IO.File.ReadAllLines fileName |> List.ofArray
        
        if not required
        then log.LogMessage(MessageImportance.Low, sprintf "Using dependency file %s" fileName)
        
        result

    with ex when Exceptions.isIORelated ex ->
        if required
        then log.LogError <| sprintf "Unable to load %s: %s: %s" fileName (ex.GetType().Name) ex.Message
        else log.LogMessage(MessageImportance.Low, sprintf "Skipping %s: %s" fileName ex.Message)

        []

let findLineSeparator (line: string) =
    let ix = line.IndexOf ':'

    if
        ix <= 0
        || ix = line.Length - 1
        || (line.[ix + 1] <> '/' && line.[ix + 1] <> '\\')
        || not <| Char.IsLetter line.[ix - 1]
        || (Seq.take ix line |> Seq.exists (not << Char.IsWhiteSpace))
    then ix
    else line.IndexOf(':', ix + 1)

let extractFileNameFromLine (line: string, beg: int, end': int) =
    try
        let end' =
            if
                beg < end'
                && end' = line.Length
                && line.[end' - 1] = '\\'
            then end' - 1
            else end'
        let fileName = line.Substring(beg, end' - beg).Trim()
        if fileName = ""
        then ""
        else IO.Path.Combine(IO.Path.GetDirectoryName fileName, IO.Path.GetFileName fileName)

    with ex when Exceptions.isIORelated ex -> ""

let readDependencyInputs (protoDepDir: string, proto: string, log: TaskLoggingHelper) =
    let depFileName = depFileNameForProto (protoDepDir, proto)
    let lines = readDepFileLines (depFileName, false, log)
    
    if List.isEmpty lines
    then []
    else
        lines
        |> List.skipWhile (fun line -> findLineSeparator line < 0)
        |> List.map (fun line ->
            let ix = findLineSeparator line
            let file = extractFileNameFromLine (line, ix + 1, line.Length)
            
            if file = ""
            then Error line
            elif file = proto
            then Ok None
            else Ok (Some file)
        )
        |> Result.sequenceListM
        |> Result.map (List.choose id)
        |> Result.unwrapWith (fun line ->
            log.LogMessage(MessageImportance.Low, sprintf "Skipping unparsable dependency file %s.\nLine with error: %s" depFileName line)
            []
        )
    
let readDependencyOutputs (depFileName: string, log: TaskLoggingHelper) =
    let lines = readDepFileLines (depFileName, true, log)
    
    if List.isEmpty lines
    then []
    else
        lines
        |> List.take (List.findIndex (fun line -> findLineSeparator line < 0) lines + 1)
        |> List.map (fun line ->
            let ix = findLineSeparator line
            let file = extractFileNameFromLine (line, 0, if ix >= 0 then ix else line.Length)
            
            if file = ""
            then Error line
            else Ok file
        )
        |> Result.sequenceListM
        |> Result.unwrapWith (fun line ->
            log.LogError <| sprintf "Unable to parse generated dependency file %s.\nLine with error: %s" depFileName line
            []
        )
