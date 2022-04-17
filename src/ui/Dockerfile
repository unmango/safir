FROM node:16 AS build

ENV CI=true

WORKDIR /app

COPY ["package.json", ".yarnrc.yml", "yarn.lock", "./"]
COPY .yarn .yarn

RUN yarn install --immutable

COPY src/ui .
RUN yarn build

FROM nginx
COPY --from=build /app/build/ /usr/share/nginx/html
