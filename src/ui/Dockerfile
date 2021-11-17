FROM node:17.1.0 AS build

ENV CI=true
WORKDIR /app
COPY .yarn/ .yarn/
COPY package.json .yarnrc.yml yarn.lock ./
RUN yarn install --immutable --check-cache

COPY . .
RUN yarn build

FROM nginx
COPY --from=build /app/build/ /usr/share/nginx/html
