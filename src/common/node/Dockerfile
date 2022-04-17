FROM node:16

ENV COMMON_ROOT=src/common/node
ENV UI_DIR=src/ui

WORKDIR /build

COPY ["package.json", "yarn.lock", ".yarnrc.yml", "./"]
COPY .yarn .yarn
COPY ${COMMON_ROOT} ${COMMON_ROOT}
COPY ${UI_DIR} ${UI_DIR}

RUN find src/* -type f ! -name "package.json" -delete && \
    find src/* -type d -empty -delete

FROM node:16

WORKDIR /build
COPY --from=0 /build .

RUN yarn install --immutable

COPY src/common/node src/common/node

# TODO: Proto build depends on the root being a git repo
RUN cd src/common/node && yarn build
