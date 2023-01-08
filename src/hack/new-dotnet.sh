#!/bin/bash
set -e

if [ $# -eq 0 ]; then
    echo "No arguments supplied"
    exit 1
fi

if [ -n "$2" ]; then
    EXTRA_ARGS="-lang $2"
fi

NAME=$1

GIT_ROOT=$(git rev-parse --show-toplevel)
REL_COMMON="src/common/dotnet"
DIR="$GIT_ROOT/$REL_COMMON/src/$NAME"

mkdir -p "$DIR"

pushd "$DIR"

dotnet new classlib $EXTRA_ARGS

cd "$GIT_ROOT/$REL_COMMON"

dotnet sln add "$DIR"

cd "$GIT_ROOT"

dotnet sln add -s common "$DIR"

popd
