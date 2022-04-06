FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /build

COPY common/dotnet/src/*/*.csproj ./
RUN find *.csproj | sed -e 's/.csproj//g' | xargs mkdir && \
    find *.csproj | sed -r -e 's/((.+).csproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'

ENV CommonSrc /build

RUN find */*.csproj | xargs -n1 dotnet restore

COPY common/dotnet/src/* ./

RUN find */*.csproj | xargs -n1 dotnet build --no-restore
