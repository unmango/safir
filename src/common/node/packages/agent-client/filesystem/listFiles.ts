import { FilesServiceClient, ListRequest } from '@unmango/safir-protos/dist/safir/agent/v1alpha1';
import { Observable, Subject } from 'rxjs';
import { Credentials, ResponseCallbacks } from '../types';

export function listFiles(
  baseUrl: string,
  callbacks?: ResponseCallbacks,
  credentials?: Credentials,
  options?: Record<string, unknown>,
): Observable<string> {
  const subject = new Subject<string>();
  const stream = new FilesServiceClient(
    baseUrl,
    credentials,
    options,
  ).list(new ListRequest());

  if (callbacks?.metadata) {
    stream.on('metadata', callbacks.metadata);
  }

  if (callbacks?.status) {
    stream.on('status', callbacks.status);
  }

  stream.on('data', x => subject.next(x.toString()));
  stream.on('error', e => subject.error(e));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};
