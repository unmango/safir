import { Observable } from 'rxjs';
import { list, listAsync } from '../filesystem';
import { Credentials, ResponseCallbacks } from '../types';

export interface FileSystemClient {
  list(callbacks?: ResponseCallbacks): Observable<string>;
  listAsync(callbacks?: ResponseCallbacks): Promise<string[]>;
}

export function createClient(
  baseUrl: string,
  credentials?: Credentials
): FileSystemClient {
  return {
    list: (c) => list(baseUrl, c, credentials),
    listAsync: (c) => listAsync(baseUrl, c, credentials),
  };
}
