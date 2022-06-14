module Safir.IO.FSharp.Path

open System.IO
open System.IO.Abstractions
open Safir.FSharp.Common

let getRelativePath = curry Path.GetRelativePath

type IPath with
    member p.getRelativePath = curry p.GetRelativePath
