import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { Observable } from 'rxjs';

// Make types pretty in the VSCode mouse-over hint text
type Normalize<T> = {
  [P in keyof T]: T[P]
}

type ExcludeEmpty<T extends unknown[]> = T extends [] ? [] :
  T extends [infer H, ...infer R] ?
  H extends Empty ? ExcludeEmpty<R> : [H, ...ExcludeEmpty<R>] :
  T;

export type ChangeReturnType<T extends Function, V> =
  T extends (...args: infer A) => any ? (...args: A) => V :
  never;

export type ServerStreaming<Req, Res> = {
  (request: Req, metadata?: Metadata): ClientReadableStream<Res>;
};

type ObservableStreamProperties<T> = {
  [P in keyof T as T[P] extends ServerStreaming<unknown, unknown> ? P : never]:
    T[P] extends ServerStreaming<unknown, infer Res>
      ? (...args: ExcludeEmpty<Parameters<T[P]>>) => Observable<Res>
      : never;
};

type AppendAsync<T extends string> = `${T & string}Async`;

type AsyncStreamProperties<T> = {
  [P in keyof T as T[P] extends ServerStreaming<unknown, unknown> ? AppendAsync<P & string> : never]:
    T[P] extends ServerStreaming<unknown, infer Res>
      ? (...args: ExcludeEmpty<Parameters<T[P]>>) => Promise<Res[]>
      : never;
};

type Unary<Request, Response> = {
  (request: Request, metadata: Metadata | null): Promise<Response>;
};

type AsyncUnaryProperties<T> = {
  [P in keyof T as T[P] extends Unary<unknown, unknown> ? AppendAsync<P & string> : never]:
    T[P] extends Unary<infer Req, infer Res>
      ? Req extends Empty // Can't use ExcludeEmpty due to unary method overloads causing weird union type
        ? (metadata?: Metadata) => Promise<Res>
        : (request: Req, metadata?: Metadata) => Promise<Res>
      : never;
};

export type ClientConstructorParams = [
  hostname: string,
  credentials?: null | { [index: string]: string; },
  options?: null | { [index: string]: any; },
];

export interface ClientConstructor<T> {
  new(...args: ClientConstructorParams): T;
}

type ClientProperties<T> =
  & ObservableStreamProperties<T>
  & AsyncStreamProperties<T>
  & AsyncUnaryProperties<T>;

export type GrpcClient<T> = Normalize<ClientProperties<T>>;
