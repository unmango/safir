FROM denoland/deno:1.11.2

WORKDIR /app

COPY src/deps.ts .
COPY import_map.json .
RUN deno cache --import-map=import_map.json deps.ts

COPY src/ ./
RUN deno cache --import-map=import_map.json server.tsx

CMD ["run", "--import-map=import_map.json", "--allow-net", "server.tsx"]
