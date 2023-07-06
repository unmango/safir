module Safir.Service.Tests.Services.FilesTests

open FsCheck.Xunit
open FsCodec.Core
open Safir.Service.Services.Files
open Swensen.Unquote

[<Property>]
let ``The event codec round-trips cleanly`` event =
    let encoded = Events.codec.Encode((), event)
    let saved = TimelineEvent.Create(0L, encoded.EventType, encoded.Data)
    let decoded = Events.codec.TryDecode(saved)
    test <@ ValueSome event = decoded @>

let (=>) events interpret =
    Fold.fold Fold.initial events |> interpret

open Events

[<Property>]
let ``Discover a file`` file =
    test <@ [] => Decisions.discover file = [ Discovered file ] @>
    test <@ [ Discovered file ] => Decisions.discover file = [] @>
    test <@ [ Discovered file; Tracked ] => Decisions.discover file = [] @>

[<Property>]
let ``Track a file`` file =
    raises <@ [] => Decisions.track @>
    test <@ [ Discovered file ] => Decisions.track = [ Tracked ] @>
    test <@ [ Discovered file; Tracked ] => Decisions.track = [] @>
