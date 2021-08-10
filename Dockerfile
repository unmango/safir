FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG GithubUsername
ARG GithubPassword

WORKDIR /build
COPY src/Safir.Manager/*.csproj .
COPY NuGet.Docker.Config NuGet.Config
RUN dotnet restore

COPY src/Safir.Manager/ .
RUN dotnet publish --no-restore --configuration Release --output /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "Safir.Manager.dll" ]
