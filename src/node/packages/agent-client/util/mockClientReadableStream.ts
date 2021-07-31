import { Metadata, Status } from 'grpc-web';

export class MockClientReadableStream<T> {

  data: (response: T) => void = _ => { };
  metadata: (metadata: Metadata) => void = _ => { };
  status: (status: Status) => void = _ => { };
  error: (error: Error) => void = _ => { };
  end: () => void = () => { };

  on(eventType: 'data', callback: (response: T) => void): void;
  on(eventType: 'metadata', callback: (metadata: Metadata) => void): void;
  on(eventType: 'status', callback: (status: Status) => void): void;
  on(eventType: 'error', callback: (error: Error) => void): void;
  on(eventType: 'end', callback: () => void): void;
  on(eventType: string, callback: (r?: any) => void) {
    switch (eventType) {
      case 'data':
        this.data = callback;
        break;
      case 'metadata':
        this.metadata = callback;
        break;
      case 'status':
        this.status = callback;
        break;
      case 'error':
        this.error = callback;
        break;
      case 'end':
        this.end = callback;
        break;
    }
  }

}
