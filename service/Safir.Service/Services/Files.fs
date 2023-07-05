module Safir.Service.Services.Files

open Safir.Service
open Safir.V1alpha1

module FileId =
    let noop = ()

module Events =
    type Event = | Discovered of File

module Fold =
    type State = | Initial

module Decisions =
    let discovered file = file

type Service() =
    class
    end
