import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Metadata, Status } from 'grpc-web';
import { Observer } from 'rxjs';
import { MetadataCallback, StatusCallback } from '../types';
import { MockClientReadableStream } from '../util';
import { listFiles } from './listFiles';

jest.mock('@unmango/safir-protos/dist/agent');

const baseUrl = 'testUrl';

describe('list', () => {
  let mockStream: MockClientReadableStream<string>;
  let mockObserver: Observer<string>;

  beforeEach(() => {
    mockStream = new MockClientReadableStream<string>();
    (FileSystemClient as jest.Mock).mockImplementation(() => ({
      listFiles: () => mockStream,
    }));

    mockObserver = {
      next: jest.fn(),
      error: jest.fn(),
      complete: jest.fn(),
    };
  });

  test('calls list with credentials', () => {
    const expected = { user: 'unmango' };

    listFiles(baseUrl, undefined, expected);

    expect(FileSystemClient).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls list with options', () => {
    const expected = { something: 'option' };

    listFiles(baseUrl, undefined, undefined, expected);

    expect(FileSystemClient).toHaveBeenCalledWith(baseUrl, undefined, expected);
  });

  test('completes observable when no data', () => {
    const observable = listFiles(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.end();

    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).toHaveBeenCalled();
  });

  test('calls error when error occurrs', () => {
    const expectedError = new Error();

    const observable = listFiles(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.error(expectedError);

    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.error).toHaveBeenCalledWith(expectedError);
  });

  test('calls next when data is received', () => {
    const expectedResult = 'data';

    const observable = listFiles(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.data(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).toHaveBeenCalledWith(expectedResult);
  });

  test('invokes callback when metadata is received', () => {
    const callback: MetadataCallback = jest.fn();
    const expectedResult: Metadata = {
      test: 'test',
    };

    const observable = listFiles(baseUrl, { metadata: callback });
    observable.subscribe(mockObserver);

    mockStream.metadata(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(callback).toHaveBeenCalledWith(expectedResult);
  });

  test('invokes callback when status is received', () => {
    const callback: StatusCallback = jest.fn();
    const expectedResult: Status = {
      code: 1995,
      details: 'birthday',
    };

    const observable = listFiles(baseUrl, { status: callback });
    observable.subscribe(mockObserver);

    mockStream.status(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(callback).toHaveBeenCalledWith(expectedResult);
  });
});
