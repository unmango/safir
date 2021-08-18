import { FileSystemClient } from '@unmango/safir-grpc-client/dist/agent';

const baseUrl = process.env.REACT_APP_AGENT_URL ?? '';
export const fileSystem = new FileSystemClient(baseUrl);
