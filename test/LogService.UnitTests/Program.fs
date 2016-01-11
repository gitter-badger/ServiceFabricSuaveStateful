open Fuchu
open Fuchu.FuchuFsCheck


let logicTests = 
    testList "Log Service Test" []


[<EntryPoint>]
let main argv = 
    runParallel logicTests