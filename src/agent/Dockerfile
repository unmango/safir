FROM safir-common-dotnet AS common
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG COMMON_DIR=/common

ENV DOTNET_NOLOGO true
ENV DOTNET_CLI_TELEMETRY_OPTOUT true

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-grpc

WORKDIR /build

ENV CommonSrc ${COMMON_DIR}
ENV AgentSrc /build

COPY --from=common /common ${COMMON_DIR}
COPY agent/src/Safir.Agent/*.csproj ./Safir.Agent/
COPY agent/src/Safir.Agent.Abstractions/*.csproj ./Safir.Agent.Abstractions/

RUN dotnet restore Safir.Agent

COPY agent/src/Safir.Agent ./Safir.Agent/
COPY agent/src/Safir.Agent.Abstractions ./Safir.Agent.Abstractions/
COPY Directory.Build.props .

ENV MinVerSkip true

RUN dotnet build Safir.Agent

# RUN dotnet publish Safir.Agent --no-restore --configuration Release --output /out

# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /app
# COPY --from=build /out .
# ENTRYPOINT [ "dotnet", "Safir.Agent.dll" ]
