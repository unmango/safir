#!/bin/bash
set -e

read -p 'Name: ' NAME
read -p 'Test? [Y/n] (Default n): ' TEST
TEST=${TEST:-n}

if [[ "${TEST,,}" == "y" ]]; then
    TARG="xunit";
else
    TARG="classlib";
fi

read -p 'Lang? [C#/F#] (Default C#): ' PLANG
PLANG=${PLANG:-C#}

GIT_ROOT=$(git rev-parse --show-toplevel)
REL_COMMON="src/common/dotnet"
DIR="$GIT_ROOT/$REL_COMMON/src/$NAME"

mkdir -p "$DIR"
pushd "$DIR"
dotnet new $TARG -lang $PLANG

cd "$GIT_ROOT/$REL_COMMON"
dotnet sln add "$DIR"

cd "$GIT_ROOT"
dotnet sln add -s common "$DIR"

popd
