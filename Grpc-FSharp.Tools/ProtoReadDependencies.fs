namespace Grpc.FSharp.Tools

open Microsoft.Build.Framework
open Microsoft.Build.Utilities

type ProtoReadDependencies() =
    inherit Task()

    [<Required>]
    member val Protobuf = Array.empty<ITaskItem> with get, set

    [<Required>]
    member val ProtoDepDir = "" with get, set

    [<Output>]
    member val Dependencies = Array.empty<ITaskItem> with get, set

    override me.Execute() =
        me.Dependencies <-
            if isNull me.ProtoDepDir
            then Array.empty
            else
                me.Protobuf
                |> Seq.map (fun proto ->
                    let deps = DependencyFile.readDependencyInputs(me.ProtoDepDir, proto.ItemSpec, me.Log)
                    deps
                    |> List.map (fun dep ->
                        let t = TaskItem(dep)
                        t.SetMetadata(Metadata.source, proto.ItemSpec)
                        t :> ITaskItem
                    )
                )
                |> Seq.concat
                |> Array.ofSeq

        not me.Log.HasLoggedErrors
