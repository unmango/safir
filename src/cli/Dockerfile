ARG CommonImage=safir-common-dotnet

FROM ${CommonImage} AS build

ENV CliSrc=/build

WORKDIR /build

COPY cli/src/Safir.CommandLine/*.csproj ./Safir.CommandLine/
COPY cli/src/Safir.Cli/*.csproj ./Safir.Cli/

RUN dotnet restore Safir.Cli

COPY cli/src/Safir.CommandLine ./Safir.CommandLine
COPY cli/src/Safir.Cli ./Safir.Cli/

RUN dotnet publish Safir.Cli \
        --no-restore \
        --configuration Release \
        --output /out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /out .
ENV SAFIR_CONFIG__DIRECTORY=/config
VOLUME "/config"
ENTRYPOINT [ "dotnet", "Safir.Cli.dll" ]
