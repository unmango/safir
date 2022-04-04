#!/bin/bash

set -e

VER=$(curl -s "https://api.github.com/repos/grpc/grpc-web/releases/latest" | grep -Po '"tag_name": "\K.*?(?=")');
URL="https://github.com/grpc/grpc-web/releases/download/$VER/protoc-gen-grpc-web-$VER-linux-x86_64";
BINDIR="$RUNNER_TOOL_CACHE"
BIN="$BINDIR/protoc-gen-grpc-web";

echo "VER: $VER"
echo "URL: $URL"
echo "BINDIR: $BINDIR"
echo "BIN: $BIN"

if [ ! -d "$BINDIR" ]; then
    echo "Creating BINDIR"
    mkdir -p $BINDIR;
fi

curl -sSL -o $BIN $URL
chmod +x $BIN
echo "$BINDIR" >> $GITHUB_PATH
