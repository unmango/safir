module Safir.Service.Ingester

open System

type Outcome =
    | Ok of used: int * unused: int
    | Skipped of count: int
    | NotApplicable of count: int

let statsInterval = TimeSpan.FromMinutes 1
let stateInterval = TimeSpan.FromMinutes 5

type Stats(logger) =
    inherit Propulsion.Streams.Stats<Outcome>(logger, statsInterval, stateInterval)

    override this.HandleOk(outcome) = () // TODO
    override this.HandleExn(logger, ex) = logger.Information(ex, "Unhandled")

let handle stream span = failwith "TODO"
