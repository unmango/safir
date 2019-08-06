FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Safir.FileManager.Rest/Safir.FileManager.Rest.csproj", "src/Safir.FileManager.Rest/"]
RUN dotnet restore "src/Safir.FileManager.Rest/Safir.FileManager.Rest.csproj"
COPY . .
WORKDIR "/src/src/Safir.FileManager.Rest"
RUN dotnet build "Safir.FileManager.Rest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Safir.FileManager.Rest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Safir.FileManager.Rest.dll"]