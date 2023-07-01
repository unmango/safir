module Safir.Service.MediaDir

open Equinox
open FSharp.UMX
open Safir.V1alpha1

type MediaDirId = string<mediaDirId>
and [<Measure>] mediaDirId

module MediaDirId =
    let inline toString (id: MediaDirId) : string = %id

[<Literal>]
let Category = "MediaDir"

let streamId = StreamId.gen MediaDirId.toString

module Events =
    type Event =
        | Discovered of Entry
