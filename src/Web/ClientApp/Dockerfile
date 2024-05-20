FROM node:18-alpine as build

ARG NODE_ENV=production
ARG REACT_APP_API_BASE_URL=""

ENV NODE_ENV $NODE_ENV
ENV REACT_APP_API_BASE_URL $REACT_APP_API_BASE_URL

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm ci

COPY . .

RUN npm run build

FROM node:18-alpine as final

WORKDIR /app

RUN npm install -g serve

COPY --from=build /app/build .

EXPOSE 3000

CMD [ "serve", "-s", "/app" ]