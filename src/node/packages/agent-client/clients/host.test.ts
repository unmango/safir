import { agent } from '@unmango/safir-protos';
import { createClient, getInfoAsync } from './host';

jest.mock('@unmango/safir-protos');

const baseUrl = 'testUrl';
const mock = jest.fn();
beforeEach(() => {
  (agent.HostClient as jest.Mock).mockImplementation(() => ({
    getInfo: mock
  }));
});

describe('createClient', () => {
  test('calls getInfo with baseUrl', async () => {
    const client = createClient(baseUrl);

    await client.getInfoAsnc();

    expect(agent.HostClient).toHaveBeenCalledWith(baseUrl);
  });
});

describe('getInfoAsync', () => {
  test('returns host info', async () => {
    const expected = {};
    mock.mockReturnValue(expected);

    const result = await getInfoAsync(baseUrl);

    expect(result).toBe(expected);
  });
});
