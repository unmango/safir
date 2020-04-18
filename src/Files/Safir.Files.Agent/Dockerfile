#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Files/Safir.Files.Agent/Safir.Files.Agent.csproj", "src/Files/Safir.Files.Agent/"]
RUN dotnet restore "src/Files/Safir.Files.Agent/Safir.Files.Agent.csproj"
COPY . .
WORKDIR "/src/src/Files/Safir.Files.Agent"
RUN dotnet build "Safir.Files.Agent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Safir.Files.Agent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Safir.Files.Agent.dll"]