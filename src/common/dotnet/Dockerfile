FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV DOTNET_NOLOGO true
ENV DOTNET_CLI_TELEMETRY_OPTOUT true
ENV MinVerSkip true
ENV CommonSrc /build
ENV ProtoRoot /protos

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-grpc

WORKDIR /build

COPY common/dotnet/src/*/*.csproj ./
RUN find *.csproj | sed -e 's/.csproj//g' | xargs mkdir && \
    find *.csproj | sed -r -e 's/((.+).csproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'

RUN find */*.csproj | xargs -n1 dotnet restore

COPY protos ${ProtoRoot}
COPY common/dotnet/src/ ./
COPY Directory.Build.props .

RUN find */*.csproj | xargs -n1 dotnet build --no-restore
