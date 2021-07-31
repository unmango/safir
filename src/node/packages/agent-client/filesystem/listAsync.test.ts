import { EMPTY, from, throwError } from 'rxjs';
import { list } from './list';
import { listAsync } from './listAsync';

jest.mock('./list');

const baseUrl = 'testUrl';

describe('listAsync', () => {
  test('calls list with credentials', async () => {
    (list as jest.Mock).mockReturnValue(from(''));
    const expected = { user: 'unmango' };

    await listAsync(baseUrl, undefined, expected);

    expect(list).toHaveBeenCalledWith(baseUrl, undefined, expected);
  });

  test('returns empty array when no data', async () => {
    (list as jest.Mock).mockReturnValue(EMPTY);

    const result = await listAsync(baseUrl);

    expect(result).toStrictEqual<string[]>([]);
  });

  test('throws when an error occurrs', async () => {
    const expected = new Error();
    (list as jest.Mock).mockReturnValue(
      throwError(() => expected),
    );

    const promise = listAsync(baseUrl);

    await expect(promise).rejects.toThrowError(expected);
  });

  test('returns result when data is available', async () => {
    const expected = 'expected';
    (list as jest.Mock).mockReturnValue(
      from([expected]),
    );

    const result = await listAsync(baseUrl);

    expect(result).toContain(expected);
  });

  test('returns all data as array', async () => {
    const expected = ['first', 'second'];
    (list as jest.Mock).mockReturnValue(
      from(expected),
    );

    const result = await listAsync(baseUrl);

    expect(result).toStrictEqual(expected);
  });
});
