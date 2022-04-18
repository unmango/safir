ARG BaseImage=safir-common-node

FROM ${BaseImage} AS build

ENV CI=true

COPY src/ui src/ui
RUN cd src/ui && yarn build

FROM nginx
COPY --from=build /build/src/ui/build/ /usr/share/nginx/html
EXPOSE 80
RUN ls -la /usr/share/nginx/html
