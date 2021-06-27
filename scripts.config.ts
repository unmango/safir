import { DenonConfig } from "https://deno.land/x/denon@2.4.8/mod.ts";
// import { config as env } from "https://deno.land/x/dotenv@v2.0.0/mod.ts";

const config: DenonConfig = {
  scripts: {
    start: {
      cmd: "src/server.tsx",
      desc: "Run my webserver",
      // env: env(),
      allow: ["net"],
      importmap: './import_map.json',
    },
  },
};

export default config;
