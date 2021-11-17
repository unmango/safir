FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG GithubUsername
ARG GithubPassword

ENV DOTNET_NOLOGO true
ENV DOTNET_CLI_TELEMETRY_OPTOUT true

WORKDIR /build
COPY src/Safir.Agent/*.csproj ./Safir.Agent/
COPY src/Safir.Agent.Abstractions/*.csproj ./Safir.Agent.Abstractions/
COPY NuGet.Docker.Config NuGet.Config
RUN dotnet restore Safir.Agent

COPY src/Safir.Agent ./Safir.Agent/
COPY src/Safir.Agent.Abstractions ./Safir.Agent.Abstractions/
COPY Directory.Build.props .
RUN dotnet publish Safir.Agent --no-restore --configuration Release --output /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "Safir.Agent.dll" ]
