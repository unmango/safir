module Safir.IO.FSharp.Directory

open System.IO
open System.IO.Abstractions

type DirectoryAction<'a> = DirectoryAction of (IDirectory -> 'a)

let enumerateFileSystemEntries =
    Directory.EnumerateFileSystemEntries

let exists = Directory.Exists
