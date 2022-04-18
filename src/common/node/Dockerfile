FROM node:16 AS setup

ENV COMMON_ROOT=src/common/node
ENV UI_DIR=src/ui

WORKDIR /build

COPY ["package.json", "yarn.lock", ".yarnrc.yml", "./"]
COPY .yarn .yarn
COPY ${COMMON_ROOT} ${COMMON_ROOT}
COPY ${UI_DIR} ${UI_DIR}

RUN find src/* -type f ! -name "package.json" -delete && \
    find src/* -type d -empty -delete

FROM node:16-bullseye

RUN echo $PATH

RUN DEBIAN_FRONTEND="noninteractive" \
    apt-get update && \
    apt-get install -y protobuf-compiler && \
    export GRPC_WEB_VERSION="1.3.1" && \
    curl -SsL \
        "https://github.com/grpc/grpc-web/releases/download/$GRPC_WEB_VERSION/protoc-gen-grpc-web-$GRPC_WEB_VERSION-linux-x86_64" \
        -o "/usr/local/bin/protoc-gen-grpc-web" && \
    chmod +x "/usr/local/bin/protoc-gen-grpc-web"

WORKDIR /build
COPY --from=setup /build .

RUN yarn install --immutable

COPY src/protos src/protos
COPY src/common/node src/common/node

ENV GIT_ROOT=/build
RUN cd src/common/node && yarn build
