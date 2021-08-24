import { FileSystemClient } from '@unmango/safir-grpc-client/dist/agent';
import { MediaClient } from '@unmango/safir-grpc-client/dist/manager';
import { MockFileSystemClient } from './fileSystem';
import { MockMediaClient } from './media';

const agentUrl = process.env.REACT_APP_AGENT_URL ?? '';
const managerUrl = process.env.REACT_APP_MANAGER_URL ?? '';

let fileSystem: FileSystemClient;
let media: MediaClient;

if ((/true/i).test(process.env.REACT_APP_PROXY_CLIENTS || 'false')) {
  fileSystem = new MockFileSystemClient('');
  media = new MockMediaClient('');
} else {
  fileSystem = new FileSystemClient(agentUrl);
  media = new MediaClient(managerUrl);
}

export {
  fileSystem,
  media,
}
