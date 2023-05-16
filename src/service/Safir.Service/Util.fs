[<AutoOpen>]
module Safir.Service.Util

open System.Runtime.CompilerServices
open FSharp.Control
open Giraffe
open Microsoft.AspNetCore.Http

[<Extension>]
type HttpContextExtensions =
    [<Extension>]
    static member inline WriteJson(context: HttpContext, dataObj) =
        context.WriteJsonAsync(dataObj) |> Task.toAsync
