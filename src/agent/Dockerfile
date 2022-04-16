ARG CommonImage=safir-common-dotnet

FROM ${CommonImage} AS build

ENV AgentSrc=/build

WORKDIR /build

COPY agent/src/Safir.Agent/*.csproj ./Safir.Agent/
COPY agent/src/Safir.Agent.Abstractions/*.csproj ./Safir.Agent.Abstractions/

RUN dotnet restore Safir.Agent

COPY agent/src/Safir.Agent ./Safir.Agent/
COPY agent/src/Safir.Agent.Abstractions ./Safir.Agent.Abstractions/

RUN dotnet publish Safir.Agent \
        --no-restore \
        --configuration Release \
        --output /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT [ "dotnet", "Safir.Agent.dll" ]
