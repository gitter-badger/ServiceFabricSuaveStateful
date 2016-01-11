open Suave
open System.Fabric
open System.Threading
open System.Threading.Tasks
open Microsoft.ServiceFabric.Services.Runtime
open Microsoft.ServiceFabric.Services.Communication.Runtime

open Services


[<EntryPoint>]
let main argv =
    use fabricRuntime = FabricRuntime.Create()
    fabricRuntime.RegisterServiceType("LogGatewayServiceType", typeof<GatewayService>)
    Thread.Sleep Timeout.Infinite
    0