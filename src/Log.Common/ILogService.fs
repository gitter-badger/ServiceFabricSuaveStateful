namespace Log.Common

open System
open System.Collections.Generic
open System.Threading.Tasks
open Microsoft.ServiceFabric.Services.Remoting

type ILogService = 
    inherit IService
    abstract member Log : string -> Task<unit>
    abstract member ReadAll : unit -> Task<IDictionary<DateTime, string>>