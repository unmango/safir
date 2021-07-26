#!/bin/bash

set -e

# In @semantic-release/exec stderr is used to write logs

ROOT="$(realpath ../../)"
>&2 echo "$ROOT"

if [ "$(basename $ROOT)" != "node" ]; then
    >&2 echo "basename was not node"
    exit 0;
fi

RELDIR="$(realpath --relative-to=$ROOT '.')"
>&2 echo "$RELDIR"

if [ -n "$(git diff --name-only | grep $RELDIR)" ]; then
    echo "patch";
fi
