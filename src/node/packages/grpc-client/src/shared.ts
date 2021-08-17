import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { ServerStreaming } from './types';

type AsyncStreamFunction<Req, Res> = {
  (request: Req, metadata?: Metadata): Promise<Res[]>;
};

type ObservableFunction<Req, Res> = {
  (request: Req, metadata?: Metadata): Observable<Res>;
};

type EmptyAsyncStreamFunction<T> = {
  (metadata?: Metadata): Promise<T[]>;
};

type EmptyObservableFunction<T> = {
  (metadata?: Metadata): Observable<T>;
};

export const toAsyncStream = <T>(stream: ClientReadableStream<T>): Promise<T[]> => {
  return firstValueFrom(
    toObservable(stream).pipe(
      // TODO: Timeout
      toArray(),
    ),
    { defaultValue: [] },
  );
};

export const toObservable = <T>(stream: ClientReadableStream<T>): Observable<T> => {
  const subject = new Subject<T>();

  stream.on('data', d => subject.next(d));
  stream.on('error', e => subject.error(e));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export const toAsyncStreamFunction = <Req, Res>(
  fn: ServerStreaming<Req, Res>
): AsyncStreamFunction<Req, Res> => {
  return (request, metadata) => {
    const stream = fn(request, metadata);
    return toAsyncStream(stream);
  }
}

export const toObservableFunction = <Req, Res>(
  fn: ServerStreaming<Req, Res>
): ObservableFunction<Req, Res> => {
  return (request, metadata) => {
    const stream = fn(request, metadata);
    return toObservable(stream);
  };
};

export const toEmptyAsyncStreamFunction = <T>(
  fn: ServerStreaming<Empty, T>
): EmptyAsyncStreamFunction<T> => {
  return (metadata) => {
    const stream = fn(new Empty(), metadata);
    return toAsyncStream(stream);
  }
}

export const toEmptyObservableFunction = <T>(
  fn: ServerStreaming<Empty, T>
): EmptyObservableFunction<T> => {
  return (metadata) => {
    const stream = fn(new Empty(), metadata);
    return toObservable(stream);
  };
};
