namespace Safir.FileManager.Store

open Safir.Common.State

module RootReducer =
    let Value = {
        Media : Reducer.Create(InitialState.Value)
    }
