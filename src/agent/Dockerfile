FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
VOLUME "/data"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS setup

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV MinVerSkip=true
ENV CommonSrc=/src
ENV AgentSrc=/src
ENV ProtoRoot=/protos

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-grpc

FROM setup AS restore

COPY common/dotnet/src/Safir.Common.FSharp/*.fsproj ${CommonSrc}/Safir.Common.FSharp/
COPY common/dotnet/src/Safir.IO.FSharp/*.fsproj ${CommonSrc}/Safir.IO.FSharp/
COPY common/dotnet/src/Safir.Common/*.csproj ${CommonSrc}/Safir.Common/
COPY common/dotnet/src/Safir.Common.Abstractions/*.csproj ${CommonSrc}/Safir.Common.Abstractions/
COPY common/dotnet/src/Safir.Messaging.Abstractions/*.csproj ${CommonSrc}/Safir.Messaging.Abstractions/
COPY common/dotnet/src/Safir.Messaging/*.csproj ${CommonSrc}/Safir.Messaging/
COPY common/dotnet/src/Safir.Protos/*.csproj ${CommonSrc}/Safir.Protos/
COPY common/dotnet/src/Safir.Redis/*.csproj ${CommonSrc}/Safir.Redis/
COPY agent/src/Safir.Agent/*.fsproj ${AgentSrc}/Safir.Agent/

RUN dotnet restore ${AgentSrc}/Safir.Agent/Safir.Agent.fsproj

FROM restore AS build

COPY protos ${ProtoRoot}
COPY Directory.Build.props ${AgentSrc}
COPY common/dotnet/src/Safir.Common.FSharp/ ${CommonSrc}/Safir.Common.FSharp/
COPY common/dotnet/src/Safir.IO.FSharp/ ${CommonSrc}/Safir.IO.FSharp/
COPY common/dotnet/src/Safir.Common/ ${CommonSrc}/Safir.Common/
COPY common/dotnet/src/Safir.Common.Abstractions/ ${CommonSrc}/Safir.Common.Abstractions/
COPY common/dotnet/src/Safir.Messaging.Abstractions/ ${CommonSrc}/Safir.Messaging.Abstractions/
COPY common/dotnet/src/Safir.Messaging/ ${CommonSrc}/Safir.Messaging/
COPY common/dotnet/src/Safir.Protos/ ${CommonSrc}/Safir.Protos/
COPY common/dotnet/src/Safir.Redis/ ${CommonSrc}/Safir.Redis/
COPY agent/src/Safir.Agent/ ${AgentSrc}/Safir.Agent/

WORKDIR ${AgentSrc}/Safir.Agent
RUN dotnet build "Safir.Agent.fsproj" \
      --no-restore \
      --configuration Release

FROM build AS publish

WORKDIR ${AgentSrc}/Safir.Agent
RUN dotnet publish "Safir.Agent.fsproj" \
      --no-restore \
      --no-build \
      --configuration Release \
      --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Safir.Agent.dll"]
