namespace Grpc.FSharp.Tools

open System
open System.Text.RegularExpressions
open Microsoft.Build.Framework
open Microsoft.Build.Utilities

type private ErrorListFilter = Regex * (TaskLoggingHelper -> Match -> unit)

type private ProtocResponseFileBuilder = private {
    Data: Text.StringBuilder
} with
    static member Create() = { Data = Text.StringBuilder(1000) }

    member me.GetContent() = me.Data.ToString()

    member me.AddSwitchMaybe (name: string, value: string) =
        if not <| String.IsNullOrEmpty value
        then 
            me.Data
                .Append("--").Append(name)
                .Append("=").Append(value)
                .Append('\n')
            |> ignore

    member me.AddSwitchMaybe (name: string, values: string array) =
        if not <| isNull values && values.Length > 0
        then 
            me.Data
                .Append("--").Append(name)
                .Append("=").Append(String.concat "," values)
                .Append('\n')
            |> ignore

    member me.AddArg(arg: string) = me.Data.Append(arg).Append('\n') |> ignore

type ProtoCompile() =
    inherit ToolTask()

    let regexTimeout = TimeSpan.FromMilliseconds(100.)

    let errorListFilters : ErrorListFilter list = [
        (
            Regex(
                "^(?'FILENAME'.+?)\\((?'LINE'\\d+)\\) ?: ?warning in column=(?'COLUMN'\\d+) ?: ?(?'TEXT'.*)",
                RegexOptions.Compiled ||| RegexOptions.IgnoreCase, regexTimeout
            ),
            fun log match' ->
                let _, line = Int32.TryParse(match'.Groups.["LINE"].Value)
                let _, column = Int32.TryParse(match'.Groups.["COLUMN"].Value)
                log.LogWarning(null, null, match'.Groups.["FILENAME"].Value, line, column, 0, 0, match'.Groups.["TEXT"].Value)
        )
        (
            Regex(
                "^(?'FILENAME'.+?)\\((?'LINE'\\d+)\\) ?: ?error in column=(?'COLUMN'\\d+) ?: ?(?'TEXT'.*)",
                RegexOptions.Compiled ||| RegexOptions.IgnoreCase, regexTimeout
            ),
            fun log match' ->
                let _, line = Int32.TryParse(match'.Groups.["LINE"].Value)
                let _, column = Int32.TryParse(match'.Groups.["COLUMN"].Value)
                log.LogError(null, null, match'.Groups.["FILENAME"].Value, line, column, 0, 0, match'.Groups.["TEXT"].Value)
        )
        (
            Regex(
                "^(?'FILENAME'.+?): ?warning: ?(?'TEXT'.*)",
                RegexOptions.Compiled ||| RegexOptions.IgnoreCase, regexTimeout
            ),
            fun log match' -> log.LogWarning(null, null, match'.Groups.["FILENAME"].Value, 0, 0, 0, 0, match'.Groups.["TEXT"].Value)
        )
        (
            Regex(
                "^(?'FILENAME'.+?): ?(?'TEXT'.*)",
                RegexOptions.Compiled ||| RegexOptions.IgnoreCase, regexTimeout
            ),
            fun log match' -> log.LogError(null, null, match'.Groups.["FILENAME"].Value, 0, 0, 0, 0, match'.Groups.["TEXT"].Value)
        )
    ]

    let trimEndSlash (dir: string) =
        if String.IsNullOrEmpty dir
        then dir
        else
            let trimmed = dir.TrimEnd('/', '\\')
            if trimmed.Length = 0
            then dir.Substring(0, 1)
            elif trimmed.Length = 2 && dir.Length > 2 && trimmed.[1] = ':' // Windows drive root
            then dir.Substring 3
            else trimmed

    [<Required>]
    member val Protobuf = Array.empty<ITaskItem> with get, set

    member val ProtoDepDir : string = null with get, set

    [<Output>]
    member val DependencyOut : string = null with get, set

    member val ProtoPath = Array.empty<string> with get, set

    [<Required>]
    member val OutputDir = "" with get, set

    member val OutputOptions = Array.empty<string> with get, set

    [<Output>]
    member val AdditionalFileWrites = Array.empty<ITaskItem> with get, set

    [<Output>]
    member val GeneratedFiles = Array.empty<ITaskItem> with get, set

    // The Grpc.Tools package source has this to say:
    // Hide this property from MSBuild, we should never use a shell script.
    member val private UseCommandProcessor = false with get, set

    override _.ToolName = if Platform.isWindows then "protoc.exe" else "protoc"

    override _.ResponseFileEncoding = upcast Text.UTF8Encoding(false)

    override me.GenerateFullPathToTool() = me.ToolName

    override _.StandardErrorLoggingImportance = MessageImportance.High

    override me.ValidateParameters() =
        if not <| isNull me.ProtoDepDir && not <| isNull me.DependencyOut
        then me.Log.LogError "Properties ProtoDepDir and DependencyOut may not be both specified"

        if me.Protobuf.Length > 1 && (not <| isNull me.ProtoDepDir || not <| isNull me.DependencyOut)
        then
            me.Log.LogError("Proto compiler currently allows only one input when \
                            --dependency_out is specified (via ProtoDepDir or DependencyOut). \
                            Tracking issue: https://github.com/google/protobuf/pull/3959")

        if not <| isNull me.ProtoDepDir
        then me.DependencyOut <- DependencyFile.depFileNameForProto (me.ProtoDepDir, me.Protobuf.[0].ItemSpec)

        not <| me.Log.HasLoggedErrors && base.ValidateParameters()

    override me.GenerateResponseFileCommands() =
        let cmd = ProtocResponseFileBuilder.Create()
        cmd.AddSwitchMaybe("fsharp_out", trimEndSlash me.OutputDir)
        cmd.AddSwitchMaybe("fsharp_opt", me.OutputOptions)

        if not <| isNull me.ProtoPath
        then me.ProtoPath |> Seq.iter (fun path -> cmd.AddSwitchMaybe("proto_path", trimEndSlash path))

        cmd.AddSwitchMaybe("dependency_out", me.DependencyOut);
        cmd.AddSwitchMaybe("error_format", "msvs")

        me.Protobuf |> Seq.iter (fun proto -> cmd.AddArg proto.ItemSpec)

        cmd.GetContent()

    override me.LogToolCommand (cmd: string) =
        let printer = Text.StringBuilder(1024)

        let quotable = [| ' '; '!'; '$'; '&'; '\''; '^' |]
        let printQuoting (str: string, start: int, count: int) =
            let wrap = count = 0 || str.IndexOfAny(quotable, start, count) >= 0
            if wrap then printer.Append '"' |> ignore
            printer.Append(str, start, count) |> ignore
            if wrap then printer.Append '"' |> ignore

        let mutable ib = 0
        let mutable ie = cmd.IndexOf('\n')
        while ie >= 0 do
            if ib = 0
            then
                let iep = cmd.IndexOf(" --");
                if iep > 0
                then
                    printQuoting(cmd, 0, iep);
                    ib <- iep + 1;
            printer.Append(' ') |> ignore
            if cmd.[ib] = '-'
            then
                let iarg = cmd.IndexOf('=', ib, ie - ib);
                if iarg < 0
                then
                    printer.Append(cmd, ib, ie - ib) |> ignore
                else
                    printer.Append(cmd, ib, iarg + 1 - ib) |> ignore
                    ib <- iarg + 1

                    printQuoting(cmd, ib, ie - ib)
            else
                printQuoting(cmd, ib, ie - ib)

            ib <- ie + 1
            ie <- cmd.IndexOf('\n', ib)

        base.LogToolCommand(printer.ToString())

    override me.LogEventsFromTextOutput(singleLine: string, messageImportance: MessageImportance) =
        let result =
            errorListFilters
            |> Seq.tryPick (fun (regex, action) ->
                let match' = regex.Match(singleLine)
                if match'.Success
                then
                    action me.Log match'
                    Some ()
                else None
            )

        match result with
        | Some () -> ()
        | None -> base.LogEventsFromTextOutput(singleLine, messageImportance)

    override me.Execute() =
        base.UseCommandProcessor <- false

        let ok = base.Execute()

        if ok && (not <| isNull me.DependencyOut)
        then
            let outputs = DependencyFile.readDependencyOutputs(me.DependencyOut, me.Log)
            if me.HasLoggedErrors
            then false
            else
                me.GeneratedFiles <- outputs |> Seq.map (fun out -> TaskItem(out) :> ITaskItem) |> Seq.toArray
                me.AdditionalFileWrites <- [| TaskItem(me.DependencyOut) :> ITaskItem |]
                true
        else ok
