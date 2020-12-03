module EnumConverter

open System.Collections.Generic

let writeEnum (ctx: FileContext) (enum: Enum) =
    ctx.Writer.WriteLine $"type {enum.Name.Value} ="

    let usedNames = HashSet<string>()
    let usedNumbers = HashSet<int32>()
    enum.Value |> Seq.iter (fun v ->
        let originalName = v.Name
        let name = Helpers.enumValueName (enum.Name.Value, v.Name.Value)
        let name = // If duplicate names result from prefix removal, we add underscores the same way the C# code generator does
            Seq.initInfinite id
            |> Seq.map (fun i -> name + String.replicate i "_")
            |> Seq.filter usedNames.Add
            |> Seq.head
        let number = v.Number.Value
        let numberUsed = not <| usedNumbers.Add number
        let preferredAlias = if numberUsed then ", PreferredAlias = false" else ""
        ctx.Writer.WriteLine $"| [<global.Google.Protobuf.Reflection.OriginalName(\"{originalName.Value}\"{preferredAlias})>] {name} = {number}"
    )
