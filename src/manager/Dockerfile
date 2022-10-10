ARG CommonImage=safir-common-dotnet

FROM ${CommonImage} AS build

ENV AgentSrc=/build
ENV ManagerSrc=/build

WORKDIR /build

COPY agent/src/Safir.Agent.Client/*.csproj ./Safir.Agent.Client/
COPY agent/src/Safir.Agent.Client.Abstractions/*.csproj ./Safir.Agent.Client.Abstractions/
COPY manager/src/Safir.Manager/*.csproj ./Safir.Manager/
COPY manager/src/Safir.Manager.Abstractions/*.csproj ./Safir.Manager.Abstractions/

RUN dotnet restore Safir.Manager

COPY agent/src/Safir.Agent.Client/ ./Safir.Agent.Client/
COPY agent/src/Safir.Agent.Client.Abstractions/ ./Safir.Agent.Client.Abstractions/
COPY manager/src/Safir.Manager/ ./Safir.Manager/
COPY manager/src/Safir.Manager.Abstractions ./Safir.Manager.Abstractions/

RUN dotnet publish Safir.Manager \
        --no-restore \
        --configuration Release \
        --output /out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT [ "dotnet", "Safir.Manager.dll" ]
