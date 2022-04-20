ARG BaseImage=safir-common-node

FROM ${BaseImage} AS build

COPY src/ui src/ui

ENV CI=true \
    BUILD_PATH=/out
RUN cd src/ui && yarn build

FROM nginx
COPY --from=build /out /usr/share/nginx/html
EXPOSE 80
