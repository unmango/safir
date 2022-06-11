module Safir.IO.FSharp.Directory

open System.IO

let enumerateFileSystemEntries = Directory.EnumerateFileSystemEntries

let exists = Directory.Exists
