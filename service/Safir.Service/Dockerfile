ARG UID=1001
ARG GID=0

# Publish application
FROM mcr.microsoft.com/dotnet/sdk:8.0.100-preview.5 AS build-env
ARG UID
ARG GID
USER 0
COPY --from=mcr.microsoft.com/dotnet/aspnet:8.0.0-preview.5 /etc/passwd /etc/group /scratch/etc
RUN grep ":$GID:" /scratch/etc/group || echo "app:x:$GID:" >>/scratch/etc/group && mkdir -p /rootfs/etc && cp /scratch/etc/group /rootfs/etc && \
    grep ":x:$UID:" /scratch/etc/passwd || echo "app:x:$UID:$GID::/home/app:/usr/sbin/nologin" >>/scratch/etc/passwd && mkdir -p /rootfs/etc && cp /scratch/etc/passwd /rootfs/etc
RUN mkdir -p /rootfs/home/app
ENV HOME=/home/build
WORKDIR /src
COPY . ./
RUN --mount=type=cache,id=nuget,target=/home/build/.nuget/packages dotnet restore .
RUN --mount=type=cache,id=nuget,target=/home/build/.nuget/packages dotnet publish --no-restore -c Release -o /rootfs/app .
RUN chgrp -R $GID /rootfs/app /rootfs/home/app && chmod -R g=u /rootfs/app /rootfs/home/app && chown -R $UID:$GID /rootfs/app /rootfs/home/app

# Build application image
FROM mcr.microsoft.com/dotnet/aspnet:8.0.0-preview.5
ARG UID
ARG GID
COPY --from=build-env /rootfs /
USER $UID:$GID
ENV ASPNETCORE_URLS=http://*:8080 \
    HOME=/home/app
WORKDIR /app
ENTRYPOINT ["dotnet", "/app/Safir.Service.dll"]
