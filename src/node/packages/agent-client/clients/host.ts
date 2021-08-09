import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import * as protos from '@unmango/safir-protos';

export interface HostClient {
  getInfoAsync(): Promise<protos.HostInfo>;
}

const client = (baseUrl: string): protos.HostClient => {
  return new protos.HostClient(baseUrl);
};

export function createClient(baseUrl: string): HostClient {
  return {
    getInfoAsync: () => getInfoAsync(baseUrl),
  };
}

export function getInfoAsync(baseUrl: string): Promise<protos.HostInfo> {
  return client(baseUrl).getInfo(new Empty(), null);
}
