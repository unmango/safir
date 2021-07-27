import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { agent } from '@unmango/safir-protos';

export interface HostClient {
  getInfoAsnc(): Promise<agent.HostInfo>;
}

const client = (baseUrl: string): agent.HostClient => {
  return new agent.HostClient(baseUrl);
};

export function createClient(baseUrl: string): HostClient {
  return {
    getInfoAsnc: () => getInfoAsync(baseUrl),
  };
}

export function getInfoAsync(baseUrl: string): Promise<agent.HostInfo> {
  return client(baseUrl).getInfo(new Empty(), null);
}
