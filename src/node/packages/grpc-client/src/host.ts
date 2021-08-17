import { HostClient, HostInfo } from '@unmango/safir-protos';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { Metadata } from 'grpc-web';
import { ClientConstructorParams, GrpcClient } from './types';

interface Interface extends GrpcClient<HostClient> { }

class Client implements Interface {

  private readonly client: HostClient;

  constructor(...args: ClientConstructorParams) {
    this.client = new HostClient(...args);
  }

  getInfoAsync(metadata?: Metadata): Promise<HostInfo> {
    return this.client.getInfo(new Empty(), metadata ?? null);
  }

}

export {
  Client as HostClient
}
