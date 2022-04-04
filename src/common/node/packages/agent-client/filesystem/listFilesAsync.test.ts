import { EMPTY, from, throwError } from 'rxjs';
import { listFiles } from './listFiles';
import { listFilesAsync } from './listFilesAsync';

jest.mock('./listFiles');

const baseUrl = 'testUrl';

describe('listAsync', () => {
  test('calls list with credentials', async () => {
    (listFiles as jest.Mock).mockReturnValue(from(''));
    const expected = { user: 'unmango' };

    await listFilesAsync(baseUrl, undefined, expected);

    expect(listFiles).toHaveBeenCalledWith(baseUrl, undefined, expected);
  });

  test('returns empty array when no data', async () => {
    (listFiles as jest.Mock).mockReturnValue(EMPTY);

    const result = await listFilesAsync(baseUrl);

    expect(result).toStrictEqual<string[]>([]);
  });

  test('throws when an error occurrs', async () => {
    const expected = new Error();
    (listFiles as jest.Mock).mockReturnValue(
      throwError(() => expected),
    );

    const promise = listFilesAsync(baseUrl);

    await expect(promise).rejects.toThrowError(expected);
  });

  test('returns result when data is available', async () => {
    const expected = 'expected';
    (listFiles as jest.Mock).mockReturnValue(
      from([expected]),
    );

    const result = await listFilesAsync(baseUrl);

    expect(result).toContain(expected);
  });

  test('returns all data as array', async () => {
    const expected = ['first', 'second'];
    (listFiles as jest.Mock).mockReturnValue(
      from(expected),
    );

    const result = await listFilesAsync(baseUrl);

    expect(result).toStrictEqual(expected);
  });
});
