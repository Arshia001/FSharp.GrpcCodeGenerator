namespace Grpc.FSharp.Tools

open Microsoft.Build.Framework
open Microsoft.Build.Utilities

type ProtoCompilerOutputs() =
    inherit Task()

    [<Required>]
    member val Protobuf = Array.empty<ITaskItem> with get, set

    [<Output>]
    member val PatchedProtobuf = Array.empty<ITaskItem> with get, set

    [<Output>]
    member val PossibleOutputs = Array.empty<ITaskItem> with get, set

    override me.Execute() =
        me.PatchedProtobuf <-
            me.Protobuf
            |> Array.map (fun proto -> GeneratorServices.patchOutputDirectory(proto, me.Log))

        me.PossibleOutputs <-
            me.PatchedProtobuf
            |> Array.map (fun proto ->
                let path = GeneratorServices.outputFileName proto
                let t = TaskItem(path)
                t.SetMetadata(Metadata.source, proto.ItemSpec)
                upcast t
            )

        not me.Log.HasLoggedErrors
