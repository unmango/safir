FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG GithubUsername
ARG GithubPassword

ENV DOTNET_NOLOGO true
ENV DOTNET_CLI_TELEMETRY_OPTOUT true

WORKDIR /build
COPY src/Safir.Manager/*.csproj ./Safir.Manager/
COPY src/Safir.Manager.Abstractions/*.csproj ./Safir.Manager.Abstractions/
COPY NuGet.Docker.Config NuGet.Config
RUN dotnet restore Safir.Manager

COPY src/Safir.Manager/ ./Safir.Manager/
COPY src/Safir.Manager.Abstractions ./Safir.Manager.Abstractions/
COPY Directory.Build.props .
RUN dotnet publish Safir.Manager --no-restore --configuration Release --output /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "Safir.Manager.dll" ]
