FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV MinVerSkip=true
ENV CommonSrc=/build
ENV ProtoRoot=/protos

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-grpc

WORKDIR /build

COPY common/dotnet/src/*/*.csproj ./
COPY common/dotnet/src/*/*.fsproj ./
RUN find *.csproj | sed -e 's/.csproj//g' | xargs mkdir && \
    find *.csproj | sed -r -e 's/((.+).csproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'
RUN find *.fsproj | sed -e 's/.fsproj//g' | xargs mkdir && \
    find *.fsproj | sed -r -e 's/((.+).fsproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'

COPY Directory.Packages.props .
RUN find */*.csproj | xargs -n1 dotnet restore
RUN find */*.fsproj | xargs -n1 dotnet restore

COPY protos ${ProtoRoot}
COPY common/dotnet/src/ ./
COPY Directory.Build.props .

RUN find */*.csproj | xargs -n1 dotnet build --no-restore
RUN find */*.fsproj | xargs -n1 dotnet build --no-restore
