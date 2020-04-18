#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Files/Safir.Files.Manager.Service/Safir.Files.Manager.Service.csproj", "src/Files/Safir.Files.Manager.Service/"]
RUN dotnet restore "src/Files/Safir.Files.Manager.Service/Safir.Files.Manager.Service.csproj"
COPY . .
WORKDIR "/src/src/Files/Safir.Files.Manager.Service"
RUN dotnet build "Safir.Files.Manager.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Safir.Files.Manager.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Safir.Files.Manager.Service.dll"]