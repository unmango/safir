module Safir.IO.FSharp.Directory

open System.IO
open System.IO.Abstractions

let enumerateFileSystemEntries =
    Directory.EnumerateFileSystemEntries

let exists = Directory.Exists

type IDirectory with
    member d.enumerateFileSystemEntries (path: string) (searchPattern: string) =
        d.EnumerateFileSystemEntries(path, searchPattern)

    member d.enumerateFileSystemEntries (path: string) (searchPattern: string) (options: EnumerationOptions) =
        d.EnumerateFileSystemEntries(path, searchPattern, options)
