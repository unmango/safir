module Safir.Agent.Validation

open System

type ValidationError =
    | DataDirectoryDoesNotExist
    | DataDirectoryNotConfigured

let validationMessage =
    Map [ (DataDirectoryDoesNotExist, "Data directory does not exist")
          (DataDirectoryNotConfigured, "No data directory configured") ]

let getMessage e = validationMessage.Item e

let dataDirectory exists =
    function
    | x when String.IsNullOrWhiteSpace(x) -> Error DataDirectoryNotConfigured
    | x when exists x -> Ok x
    | _ -> Error DataDirectoryDoesNotExist
