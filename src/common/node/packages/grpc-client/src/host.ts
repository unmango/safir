import { HostServiceClient, InfoRequest, InfoResponse } from '@unmango/safir-protos/dist/safir/common/v1alpha1';
import { Metadata } from 'grpc-web';
import { ClientConstructorParams, GrpcClient } from './types';

interface Interface extends GrpcClient<HostServiceClient> { }

class Client implements Interface {

  private readonly client: HostServiceClient;

  constructor(...args: ClientConstructorParams) {
    this.client = new HostServiceClient(...args);
  }

  infoAsync(metadata?: Metadata): Promise<InfoResponse> {
    return this.client.info(new InfoRequest(), metadata ?? null);
  }

}

export {
  Client as HostClient
}
