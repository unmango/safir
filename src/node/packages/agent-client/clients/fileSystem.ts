import { Observable } from 'rxjs';
import { listFiles, listFilesAsync } from '../filesystem';
import { Credentials, ResponseCallbacks } from '../types';

export interface FileSystemClient {
  listFiles(callbacks?: ResponseCallbacks): Observable<string>;
  listFilesAsync(callbacks?: ResponseCallbacks): Promise<string[]>;
}

export function createClient(
  baseUrl: string,
  credentials?: Credentials
): FileSystemClient {
  return {
    listFiles: (c) => listFiles(baseUrl, c, credentials),
    listFilesAsync: (c) => listFilesAsync(baseUrl, c, credentials),
  };
}
