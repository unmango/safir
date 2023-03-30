ARG CommonImage=safir-common-dotnet

FROM ${CommonImage} AS build

ENV CliSrc=/build

WORKDIR /build

COPY cli/src/Safir.CommandLine/*.csproj ./Safir.CommandLine/
COPY cli/src/Safir.CommandLine.Generator/*.csproj ./Safir.CommandLine.Generator/
COPY cli/src/Safir.Cli/*.csproj ./Safir.Cli/

RUN dotnet restore Safir.Cli

COPY cli/src/Safir.CommandLine ./Safir.CommandLine
COPY cli/src/Safir.CommandLine.Generator ./Safir.CommandLine.Generator
COPY cli/src/Safir.Cli ./Safir.Cli/

RUN dotnet publish Safir.Cli \
        --no-restore \
        --configuration Release \
        --runtime linux-x64 \
        --no-self-contained \
        --output /out

FROM mcr.microsoft.com/dotnet/runtime:8.0-preview
WORKDIR /app
COPY --from=build /out .
ENV SAFIR_CONFIG__DIRECTORY="/config"
RUN mkdir $SAFIR_CONFIG__DIRECTORY
VOLUME $SAFIR_CONFIG__DIRECTORY
ENTRYPOINT [ "dotnet", "Safir.Cli.dll" ]
