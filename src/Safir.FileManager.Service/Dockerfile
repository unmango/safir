FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Safir.FileManager.Service/Safir.FileManager.Service.csproj", "src/Safir.FileManager.Service/"]
RUN dotnet restore "src/Safir.FileManager.Service/Safir.FileManager.Service.csproj"
COPY . .
WORKDIR "/src/src/Safir.FileManager.Service"
RUN dotnet build "Safir.FileManager.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Safir.FileManager.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Safir.FileManager.Service.dll"]