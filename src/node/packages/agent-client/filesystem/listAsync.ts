import { firstValueFrom, toArray } from 'rxjs';
import { Credentials, ResponseCallbacks } from '../types';
import { list } from './list';

export function listAsync(
  baseUrl: string,
  callbacks?: ResponseCallbacks,
  credentials?: Credentials,
): Promise<string[]> {
  return firstValueFrom(
    list(
      baseUrl,
      callbacks,
      credentials
    ).pipe(toArray()),
    { defaultValue: [] },
  );
};
