namespace Grpc.FSharp.Tools

open Microsoft.Build.Framework
open Microsoft.Build.Utilities

type ProtoToolsPlatform() =
    inherit Task()

    [<Output>]
    member val Os = "" with get, set

    [<Output>]
    member val Cpu = "" with get, set

    override me.Execute() =
        me.Os <-
            match Platform.os with
            | Platform.Windows -> "windows"
            | Platform.Linux -> "linux"
            | Platform.MacOSX -> "macosx"
            | Platform.UnknownOS -> ""

        me.Cpu <-
            match Platform.processorArchitecture with
            | Platform.X64 -> "x64"
            | Platform.X86 -> "x86"
            | Platform.UnknownProcessor -> ""

        true
