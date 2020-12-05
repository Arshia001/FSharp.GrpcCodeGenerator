// Learn more about F# at http://fsharp.org

open System
open Google.Protobuf
open Google.Protobuf.FSharp

[<EntryPoint>]
let main _ =
    let debugMode =
        //true
        false
    let req = 
        if debugMode
        then
            let bytes = IO.File.ReadAllBytes "req.bin"
            Compiler.CodeGeneratorRequest.Parser.ParseFrom(bytes)
        else
            use stdIn = Console.OpenStandardInput()
            let req = Compiler.CodeGeneratorRequest.Parser.ParseFrom(stdIn)
            IO.File.Delete "req.bin"
            use file = IO.File.OpenWrite "req.bin"
            req.WriteTo(file)
            req
    let resp = CodeGenerator.generate req
    if debugMode
    then
        IO.Directory.CreateDirectory "__DEBUG" |> ignore
        resp.File |> Seq.iter (fun f -> IO.File.WriteAllText(IO.Path.Combine("__DEBUG", f.Name.Value), f.Content.Value))
    use stdOut = Console.OpenStandardOutput()
    resp.WriteTo(stdOut)
    0
