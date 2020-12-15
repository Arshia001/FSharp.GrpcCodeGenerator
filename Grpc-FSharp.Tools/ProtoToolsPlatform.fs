namespace Grpc.FSharp.Tools

open Microsoft.Build.Utilities

type ProtoToolsPlatform() =
    inherit Task()

    override _.Execute() = true
