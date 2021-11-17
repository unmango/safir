import { Metadata, Status } from 'grpc-web';

export type Credentials = Record<string, string>;

export interface MetadataCallback {
  (metadata: Metadata): void;
}

export interface StatusCallback {
  (status: Status): void;
}

export interface ResponseCallbacks {
  metadata?: MetadataCallback;
  status?: StatusCallback;
}
