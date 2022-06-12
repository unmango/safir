module Safir.IO.FSharp.Path

open System.IO
open Safir.Common

let getRelativePath = curry Path.GetRelativePath
