import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@safir/protos';

export interface FileSystemClient {
  list(): Observable<string>;
  listAsync(): Promise<string[]>;
}

const client = (baseUrl: string): agent.FileSystemClient => {
  return new agent.FileSystemClient(baseUrl);
};

export function createClient(baseUrl: string): FileSystemClient {
  return {
    list: () => list(baseUrl),
    listAsync: () => listAsync(baseUrl),
  };
}

export function list(baseUrl: string): Observable<string> {
  const subject = new Subject<string>();
  const stream = client(baseUrl).list(new Empty());

  stream.on('data', x => subject.next(x as string));
  stream.on('error', x => subject.error(x));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export function listAsync(baseUrl: string): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl).pipe(toArray()),
    {
      defaultValue: []
    });
};
