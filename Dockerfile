FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /build
COPY src/Safir.Agent.Protos/*.csproj Safir.Agent.Protos/
COPY src/Safir.Agent/*.csproj Safir.Agent/
RUN dotnet restore Safir.Agent

COPY src/Safir.Agent.Protos/ Safir.Agent.Protos/
COPY src/Safir.Agent/ Safir.Agent/
RUN dotnet publish Safir.Agent --no-restore --configuration Release --output /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "Safir.Agent.dll" ]
