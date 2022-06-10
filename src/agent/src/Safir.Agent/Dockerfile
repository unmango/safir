FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/agent/src/Safir.Agent/Safir.Agent.fsproj", "Safir.Agent/"]
RUN dotnet restore "src/agent/src/Safir.Agent/Safir.Agent.fsproj"
COPY . .
WORKDIR "/src/Safir.Agent"
RUN dotnet build "Safir.Agent.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Safir.Agent.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Safir.Agent.dll"]
