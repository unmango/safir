namespace Safir.FileManager.Store

open System.Collections.Immutable

type State = {
    Media : IImmutableList<string>
}

module InitialState =
    let Value = {
        Media = ImmutableArray.Empty
    }
