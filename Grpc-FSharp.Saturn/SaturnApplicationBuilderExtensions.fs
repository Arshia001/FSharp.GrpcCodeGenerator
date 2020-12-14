module Saturn

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

type Saturn.Application.ApplicationBuilder with
    [<CustomOperation("use_grpc")>]
    /// Adds gRPC endpoint. Passed parameter should be any constructor of the gRPC service implementation.
    member _.UseGrpc<'a, 'b when 'a : not struct>(state: Saturn.Application.ApplicationState, _cons: 'b -> 'a)  =
        let configureServices(svcs: IServiceCollection) =
            svcs.AddGrpc() |> ignore
            svcs
        
        let configureApplication(app: IApplicationBuilder) =
            app
                .UseRouting()
                .UseEndpoints(fun ep -> ep.MapGrpcService<'a>() |> ignore)
        
        { state with
            AppConfigs = configureApplication :: state.AppConfigs
            ServicesConfig = configureServices :: state.ServicesConfig
        } 
