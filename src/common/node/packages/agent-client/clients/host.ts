import * as common from '@unmango/safir-protos/dist/safir/common/v1alpha1'

export interface HostClient {
  getInfoAsync(): Promise<common.InfoResponse>;
}

const client = (baseUrl: string): common.HostServiceClient => {
  return new common.HostServiceClient(baseUrl);
};

export function createClient(baseUrl: string): HostClient {
  return {
    getInfoAsync: () => infoAsync(baseUrl),
  };
}

export function infoAsync(baseUrl: string): Promise<common.InfoResponse> {
  return client(baseUrl).info(new common.InfoRequest(), null);
}
