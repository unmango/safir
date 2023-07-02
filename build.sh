#!/usr/bin/env bash

set -eu

dotnet run --project ./build/Safir.Build.fsproj -- -t "$@"
