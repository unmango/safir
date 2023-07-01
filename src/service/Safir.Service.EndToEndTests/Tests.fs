namespace Safir.Service.EndToEndTests

open System
open Xunit

type Tests(fixture: ServiceFixture) =
    [<Fact(Skip = "WIP")>]
    let ``My test`` () =
        Assert.True(true)

    interface IClassFixture<ServiceFixture>
