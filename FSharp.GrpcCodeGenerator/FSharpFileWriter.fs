[<AutoOpen>]
module FSharpFileWriter

open System

type IFSharpFileWriter =
    abstract Indent: unit -> unit
    abstract Outdent: unit -> unit
    abstract WriteLine: string -> unit
    abstract WriteLines: #seq<string> -> unit
    abstract Write: string -> unit
    abstract WriteAll: string seq -> unit
    abstract GetText: unit -> string

type private FSharpFileWriter = {
    Builder: Text.StringBuilder
    mutable Indent: int32
    mutable AtStartOfLine: bool
} with
    interface IFSharpFileWriter with
        member me.Outdent() =
            if me.Indent <= 0
            then failwith "Cannot outdent further"
            else me.Indent <- me.Indent - 1

        member me.Indent() = me.Indent <- me.Indent + 1

        member me.Write(text) =
            if text <> ""
            then
                if me.AtStartOfLine
                then
                    String.replicate me.Indent "    "
                    |> me.Builder.Append
                    |> ignore
                me.Builder.Append text |> ignore
                me.AtStartOfLine <- false

        member me.WriteAll(texts) = Seq.iter (me :> IFSharpFileWriter).Write texts

        member me.WriteLine(line) =
            (me :> IFSharpFileWriter).Write line
            me.Builder.AppendLine() |> ignore
            me.AtStartOfLine <- true

        member me.WriteLines(lines) = Seq.iter (me :> IFSharpFileWriter).WriteLine lines

        member me.GetText() = me.Builder.ToString()

module FSharpFileWriter =
    let create () =
        {
            Builder = Text.StringBuilder()
            Indent = 0
            AtStartOfLine = true
        } :> IFSharpFileWriter