import { FilesServiceClient, ListRequest, ListResponse } from '@unmango/safir-protos/dist/safir/agent/v1alpha1';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { defer, Observable } from 'rxjs';
import { toAsyncStream, toObservable } from '../shared';
import { ChangeReturnType, ClientConstructorParams, GrpcClient } from '../types';

type FixedClient = {
  [P in keyof FilesServiceClient]:
    P extends 'listFiles' ? ChangeReturnType<
      FilesServiceClient[P],
      ClientReadableStream<ListResponse>
    > :
    FilesServiceClient[P];
}

// So we can create a class that implements GrpcClient
interface Interface extends GrpcClient<FixedClient> { }

class Client implements Interface {

  private readonly client: FilesServiceClient;

  constructor(...args: ClientConstructorParams) {
    this.client = new FilesServiceClient(...args);
  }

  public list(metadata?: Metadata): Observable<ListResponse> {
    return defer(() => {
      const broken = this.client.list(new ListRequest(), metadata);
      const stream = broken as ClientReadableStream<ListResponse>;

      return toObservable(stream);
    });
  }

  public listAsync(metadata?: Metadata): Promise<ListResponse[]> {
    const broken = this.client.list(new ListRequest(), metadata);
    const stream = broken as ClientReadableStream<ListResponse>;

    return toAsyncStream(stream);
  }

}

export {
  Client as FileSystemClient
}
