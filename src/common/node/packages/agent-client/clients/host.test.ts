import { HostServiceClient } from '@unmango/safir-protos/dist/safir/common/v1alpha1';
import { createClient, infoAsync } from './host';

jest.mock('@unmango/safir-protos');

const baseUrl = 'testUrl';
const mock = jest.fn();
beforeEach(() => {
  (HostServiceClient as jest.Mock).mockImplementation(() => ({
    getInfo: mock
  }));
});

describe('createClient', () => {
  test('calls getInfo with baseUrl', async () => {
    const client = createClient(baseUrl);

    await client.getInfoAsync();

    expect(HostServiceClient).toHaveBeenCalledWith(baseUrl);
  });
});

describe('getInfoAsync', () => {
  test('returns host info', async () => {
    const expected = {};
    mock.mockReturnValue(expected);

    const result = await infoAsync(baseUrl);

    expect(result).toBe(expected);
  });
});
