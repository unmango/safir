import { firstValueFrom, toArray } from 'rxjs';
import { Credentials, ResponseCallbacks } from '../types';
import { listFiles } from './listFiles';

export function listFilesAsync(
  baseUrl: string,
  callbacks?: ResponseCallbacks,
  credentials?: Credentials,
): Promise<string[]> {
  return firstValueFrom(
    listFiles(
      baseUrl,
      callbacks,
      credentials
    ).pipe(toArray()),
    { defaultValue: [] },
  );
};
