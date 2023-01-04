#!/bin/bash
set -e

GIT_ROOT=$(git rev-parse --show-toplevel)

docker image build --no-cache -t build-context -f - "$GIT_ROOT/src" <<EOF
FROM busybox
WORKDIR /build-context
COPY . .
CMD find .
EOF

docker create --name dockerignore-test build-context
docker export dockerignore-test > dockerignore-test.tar

docker container rm dockerignore-test
docker image rm build-context
