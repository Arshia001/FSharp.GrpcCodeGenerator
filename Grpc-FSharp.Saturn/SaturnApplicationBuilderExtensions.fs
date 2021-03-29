module Saturn

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing
open Microsoft.Extensions.DependencyInjection

type Saturn.Application.ApplicationBuilder with
    [<CustomOperation("use_grpc")>]
    /// Adds gRPC endpoint. Passed parameter should be any constructor of the gRPC service implementation.
    member _.UseGrpc<'a, 'b when 'a : not struct>(state: Saturn.Application.ApplicationState, _cons: 'b -> 'a)  =
        let configureServices(svcs: IServiceCollection) =
            svcs.AddGrpc() |> ignore
            svcs
        
        let configureApplication(app: IApplicationBuilder) =
            // The EndpointRouteBuilder needs to be registered exactly once.
            if
                app.Properties.Values
                |> Seq.filter (fun x -> x :? IEndpointRouteBuilder)
                |> Seq.isEmpty
            then app.UseRouting() |> ignore

            app.UseEndpoints(fun ep -> ep.MapGrpcService<'a>() |> ignore)
        
        { state with
            AppConfigs = configureApplication :: state.AppConfigs
            ServicesConfig = configureServices :: state.ServicesConfig
        } 
