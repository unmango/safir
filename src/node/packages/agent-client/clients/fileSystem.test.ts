import { createClient } from './fileSystem';
import { listFiles, listFilesAsync } from '../filesystem';

jest.mock('@unmango/safir-protos/dist/agent');
jest.mock('../filesystem');

const baseUrl = 'testUrl';

describe('createClient', () => {
  test('calls list with baseUrl', () => {
    const client = createClient(baseUrl);

    client.listFiles();

    expect(listFiles).toHaveBeenCalledWith(baseUrl, undefined, undefined);
  });

  test('calls list with credentials', () => {
    const expected = { user: 'unmango' };
    const client = createClient(baseUrl, expected);

    client.listFiles();

    expect(listFiles).toHaveBeenCalledWith(baseUrl, undefined, expected);
  });

  test('calls listAsync with metadata callback', async () => {
    const client = createClient(baseUrl);
    const expected = { metadata: jest.fn() };

    client.listFiles(expected);

    expect(listFiles).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls list with status callback', async () => {
    const client = createClient(baseUrl);
    const expected = { status: jest.fn() };

    client.listFiles(expected);

    expect(listFiles).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls list with callbacks', async () => {
    const client = createClient(baseUrl);
    const expected = {
      metadata: jest.fn(),
      status: jest.fn(),
    };

    client.listFiles(expected);

    expect(listFiles).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls listAsync with baseUrl', async () => {
    const client = createClient(baseUrl);

    await client.listFilesAsync();

    expect(listFilesAsync).toHaveBeenCalledWith(baseUrl, undefined, undefined);
  });

  test('calls listAsync with metadata callback', async () => {
    const client = createClient(baseUrl);
    const expected = { metadata: jest.fn() };

    await client.listFilesAsync(expected);

    expect(listFilesAsync).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls listAsync with status callback', async () => {
    const client = createClient(baseUrl);
    const expected = { status: jest.fn() };

    await client.listFilesAsync(expected);

    expect(listFilesAsync).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });

  test('calls listAsync with callbacks', async () => {
    const client = createClient(baseUrl);
    const expected = {
      metadata: jest.fn(),
      status: jest.fn(),
    };

    await client.listFilesAsync(expected);

    expect(listFilesAsync).toHaveBeenCalledWith(baseUrl, expected, undefined);
  });
});
