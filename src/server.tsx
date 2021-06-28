import { serve, Server } from 'http';
import React from 'react';
import ReactDOMServer from 'react-dom/server';
import App from './App.tsx';

const server: Server = serve({ port: 3000 });
console.log(`HTTP webserver running.  Access it at:  http://localhost:3000/`);

for await (const request of server) {
  const app = ReactDOMServer.renderToString(<App />);
  request.respond({ status: 200, body: app });
}
