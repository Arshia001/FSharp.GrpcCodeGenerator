// Learn more about F# at http://fsharp.org

open System
open Google.Protobuf
open Google.Protobuf.FSharp

[<EntryPoint>]
let main _ =
    use stdIn = Console.OpenStandardInput()
    let req = Compiler.CodeGeneratorRequest.Parser.ParseFrom(stdIn)
    let resp = CodeGenerator.generate req
    use stdOut = Console.OpenStandardOutput()
    resp.WriteTo(stdOut)
    0
