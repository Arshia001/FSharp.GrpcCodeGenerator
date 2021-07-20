module Issue6

open System
open Xunit
open FSharp.GrpcCodeGenerator.TestProtos.FSharp
open Grpc.Core
open System.Threading.Tasks

type AnnotatedServiceImpl() =
    inherit AnnotatedService.AnnotatedServiceBase()

    override this.DoStuff (request: Request) (context: ServerCallContext) : Task<Response> =
        Task.FromResult(
            { Message = request.Message
              _UnknownFields = null }
        )

[<Fact>]
let ``Should be able to create a gRPC service using annotation proto`` () =
    let server = Server()

    let serviceDefinition =
        AnnotatedService.AnnotatedServiceMethodBinder.BindService(AnnotatedServiceImpl())

    server.Services.Add(serviceDefinition)
