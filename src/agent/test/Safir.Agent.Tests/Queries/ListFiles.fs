module Safir.Agent.Tests.Queries.ListFiles

open System.IO
open System.IO.Abstractions
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Moq
open Safir.Agent.Configuration
open Safir.Agent.Queries.ListFiles
open Swensen.Unquote
open Xunit

[<Fact>]
let ``Enumerate fileSystem entries uses data directory`` () =
    let expected = "expected"
    let mutable actual = ""

    let enumerate (d, _, _) =
        actual <- d
        []

    let getRelativePath _ = ""

    listFiles enumerate getRelativePath expected 0
    |> ignore

    test <@ expected = actual @>

[<Fact>]
let ``Enumerate fileSystem entries passes wildcard`` () =
    let mutable actual = ""

    let enumerate (_, s, _) =
        actual <- s
        []

    let getRelativePath _ = ""

    listFiles enumerate getRelativePath "" 0 |> ignore

    test <@ "*" = actual @>

[<Fact>]
let ``Enumerate fileSystem entries passes max depth`` () =
    let expected = 69
    let mutable actual = EnumerationOptions()

    let enumerate (_, _, m) =
        actual <- m
        []

    let getRelativePath _ = ""

    listFiles enumerate getRelativePath "" expected
    |> ignore

    test <@ expected = actual.MaxRecursionDepth @>

[<Fact>]
let ``listFiles gets relative path from data directory`` () =
    let expected = "expected"
    let mutable actual = ""
    let enumerate _ = [ "" ]

    let getRelativePath (d, _) =
        actual <- d
        ""

    listFiles enumerate getRelativePath expected 0
    |> Seq.toList
    |> ignore

    test <@ expected = actual @>

[<Fact>]
let ``listFiles gets relative path for file`` () =
    let expected = "expected"
    let mutable actual = ""
    let enumerate _ = [ expected ]

    let getRelativePath (_, f) =
        actual <- f
        ""

    listFiles enumerate getRelativePath expected 0
    |> Seq.toList
    |> ignore

    test <@ expected = actual @>

[<Fact>]
let ``listFiles maps relative paths`` () =
    let expected = "expected"
    let enumerate _ = [ expected ]
    let getRelativePath _ = expected

    let actual =
        listFiles enumerate getRelativePath expected 0
        |> Seq.toList

    test <@ expected = actual[0].Path @>

let options = Mock<IOptions<AgentOptions>>()
let directory = Mock<IDirectory>()
let path = Mock<IPath>()
let logger = Mock<ILogger<ListFiles>>()

let underTest =
    ListFiles(options.Object, directory.Object, path.Object, logger.Object)

[<Fact>]
let ``Execute returns empty on validation error`` () =
    let actual = underTest.Execute

    test <@ true = Seq.isEmpty actual @>
