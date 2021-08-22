import { FileSystemClient } from '@unmango/safir-grpc-client/dist/agent';
import { MediaClient } from '@unmango/safir-grpc-client/dist/manager';

const agentUrl = process.env.REACT_APP_AGENT_URL ?? '';
const managerUrl = process.env.REACT_APP_MANAGER_URL ?? '';

export const fileSystem = new FileSystemClient(agentUrl);
export const media = new MediaClient(managerUrl);
