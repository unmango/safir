FROM node:latest as node

WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY package.json /app/package.json
RUN npm install

COPY . /app
RUN npm run build --prod

FROM nginx:1.13.8-alpine

COPY --from=node /app/dist/ /usr/share/nginx/html
#COPY ./nginx.conf /etc/nginx/conf.d/default.conf
