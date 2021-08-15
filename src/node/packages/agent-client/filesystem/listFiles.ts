import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { Observable, Subject } from 'rxjs';
import { Credentials, ResponseCallbacks } from '../types';

export function listFiles(
  baseUrl: string,
  callbacks?: ResponseCallbacks,
  credentials?: Credentials,
  options?: Record<string, unknown>,
): Observable<string> {
  const subject = new Subject<string>();
  const stream = new FileSystemClient(
    baseUrl,
    credentials,
    options,
  ).listFiles(new Empty());

  if (callbacks?.metadata) {
    stream.on('metadata', callbacks.metadata);
  }

  if (callbacks?.status) {
    stream.on('status', callbacks.status);
  }

  stream.on('data', x => subject.next(x as string));
  stream.on('error', e => subject.error(e));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};
