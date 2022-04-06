FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG SOURCE_ROOT=/common

ENV DOTNET_NOLOGO true
ENV DOTNET_CLI_TELEMETRY_OPTOUT true

WORKDIR ${SOURCE_ROOT}

COPY common/dotnet/src/*/*.csproj ./
RUN find *.csproj | sed -e 's/.csproj//g' | xargs mkdir && \
    find *.csproj | sed -r -e 's/((.+).csproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'

ENV CommonSrc ${SOURCE_ROOT}
ENV ProtoRoot ${SOURCE_ROOT}/protos

RUN find */*.csproj | xargs -n1 dotnet restore

COPY protos .
COPY common/dotnet/src/* ./

RUN find */*.csproj | xargs -n1 dotnet build --no-restore
