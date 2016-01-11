module Services

open System
open System.Linq
open System.Collections.Generic
open System.Fabric
open System.Threading
open System.Threading.Tasks
open Microsoft.ServiceFabric.Data
open Microsoft.ServiceFabric.Data.Collections
open Microsoft.ServiceFabric.Services.Runtime
open Microsoft.ServiceFabric.Services.Remoting.Runtime
open Microsoft.ServiceFabric.Services.Communication.Runtime

open Log.Common


type StoreService() as this=
    inherit StatefulService()

    let impl = 
        { new ILogService with
            member __.Log s = 
                async {
                    let! d = this.StateManager.GetOrAddAsync<IReliableDictionary<DateTime, string>>("log") |> Async.AwaitTask
                    use tx = this.StateManager.CreateTransaction()
                    let! r = d.TryAddAsync(tx, DateTime.Now, s) |> Async.AwaitTask
                    let! res = if r then tx.CommitAsync () |> Async.AwaitTask else async {return -1L }


                    return ()
                } |> Async.StartAsTask
            member __.ReadAll () = 
                async {
                    let! d = this.StateManager.GetOrAddAsync<IReliableDictionary<DateTime, string>>("log") |> Async.AwaitTask
                    return d.CreateEnumerable().ToDictionary((fun x -> x.Key), (fun (y : KeyValuePair<DateTime,string>) -> y.Value)) :> IDictionary<_,_>   
                } |> Async.StartAsTask
        }


    override __.CreateServiceReplicaListeners() = 
        seq {
            yield ServiceReplicaListener(fun ip -> ServiceRemotingListener<ILogService>(ip, impl) :> _ )
        }


