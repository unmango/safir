module Safir.IO.FSharp.Directory

open System.IO
open System.IO.Abstractions
open Safir.Common

type DirectoryAction<'a> = Reader<IDirectory, 'a>

let enumerateFileSystemEntries =
    Directory.EnumerateFileSystemEntries

let exists = Directory.Exists
