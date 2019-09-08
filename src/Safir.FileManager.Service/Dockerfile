FROM mcr.microsoft.com/dotnet/core/runtime:3.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["Safir.FileManager.Service.csproj", .]
RUN dotnet restore "Safir.FileManager.Service.csproj"
COPY . .
RUN dotnet build "Safir.FileManager.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Safir.FileManager.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Safir.FileManager.Service.dll"]
