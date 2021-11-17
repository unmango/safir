import { MediaClient, MediaItem } from '@unmango/safir-protos/dist/manager';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { defer, Observable } from 'rxjs';
import { toAsyncStream, toObservable } from '../shared';
import { ChangeReturnType, ClientConstructorParams, GrpcClient } from '../types';

type FixedMediaClient = {
  [P in keyof MediaClient]:
    P extends 'list' ? ChangeReturnType<
      MediaClient[P],
      ClientReadableStream<MediaItem>
    > :
    MediaClient[P];
}

// So we can create a class that implements GrpcClient
interface Interface extends GrpcClient<FixedMediaClient> { }

class Client implements Interface {

  private readonly client: MediaClient;

  constructor(...args: ClientConstructorParams) {
    this.client = new MediaClient(...args);
  }

  public list(metadata?: Metadata): Observable<MediaItem> {
    return defer(() => {
      const broken = this.client.list(new Empty(), metadata);
      const stream = broken as ClientReadableStream<MediaItem>;
  
      return toObservable(stream);
    });
  }

  public listAsync(metadata?: Metadata): Promise<MediaItem[]> {
    const broken = this.client.list(new Empty(), metadata);
    const stream = broken as ClientReadableStream<MediaItem>;

    return toAsyncStream(stream);
  }

}

export {
  Client as MediaClient
}
