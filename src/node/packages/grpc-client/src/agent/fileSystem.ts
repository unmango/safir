import { FileSystemClient, FileSystemEntry } from '@unmango/safir-protos/dist/agent';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { Observable } from 'rxjs';
import { toAsyncStream, toObservable } from '../shared';
import { ChangeReturnType, ClientConstructorParams, GrpcClient } from '../types';

type FixedClient = {
  [P in keyof FileSystemClient]:
    P extends 'listFiles' ? ChangeReturnType<
      FileSystemClient[P],
      ClientReadableStream<FileSystemEntry>
    > :
    FileSystemClient[P];
}

// So we can create a class that implements GrpcClient
interface Interface extends GrpcClient<FixedClient> { }

class Client implements Interface {

  private readonly client: FileSystemClient;

  constructor(...args: ClientConstructorParams) {
    this.client = new FileSystemClient(...args);
  }

  public listFiles(metadata?: Metadata): Observable<FileSystemEntry> {
    const broken = this.client.listFiles(new Empty(), metadata);
    const stream = broken as ClientReadableStream<FileSystemEntry>;

    return toObservable(stream);
  }

  public listFilesAsync(metadata?: Metadata): Promise<FileSystemEntry[]> {
    const broken = this.client.listFiles(new Empty(), metadata);
    const stream = broken as ClientReadableStream<FileSystemEntry>;

    return toAsyncStream(stream);
  }

}

export {
  Client as FileSystemClient
}
