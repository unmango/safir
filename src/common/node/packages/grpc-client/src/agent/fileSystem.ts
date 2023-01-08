import { FilesServiceClient, FilesServiceListRequest, FilesServiceListResponse } from '@unmango/safir-protos/dist/safir/agent/v1alpha1';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { defer, Observable } from 'rxjs';
import { toAsyncStream, toObservable } from '../shared';
import { ChangeReturnType, ClientConstructorParams, GrpcClient } from '../types';

type FixedClient = {
  [P in keyof FilesServiceClient]:
    P extends 'listFiles' ? ChangeReturnType<
      FilesServiceClient[P],
      ClientReadableStream<FilesServiceListResponse>
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

  public list(metadata?: Metadata): Observable<FilesServiceListResponse> {
    return defer(() => {
      const broken = this.client.list(new FilesServiceListRequest(), metadata);
      const stream = broken as ClientReadableStream<FilesServiceListResponse>;

      return toObservable(stream);
    });
  }

  public listAsync(metadata?: Metadata): Promise<FilesServiceListResponse[]> {
    const broken = this.client.list(new FilesServiceListRequest(), metadata);
    const stream = broken as ClientReadableStream<FilesServiceListResponse>;

    return toAsyncStream(stream);
  }

}

export {
  Client as FileSystemClient
}
