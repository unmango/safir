FROM node:16.5.0 AS build

ENV CI=true
WORKDIR /app
RUN yarn set version berry
COPY package.json yarn.lock ./
RUN yarn install --immutable --check-cache

COPY . .
RUN yarn build

FROM nginx
COPY --from=build /app/build/ /usr/share/nginx/html
