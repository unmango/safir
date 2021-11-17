import { exec } from 'child_process';
import * as fs from 'fs/promises';
import glob from 'glob';
import * as path from 'path';
import { promisify } from 'util';

export const execAsync = promisify(exec);
export const globAsync = promisify(glob);

export const command = (...args: string[]): string => {
  return args.join(' ');
}

export const gitRootAsync = async (): Promise<string> => {
  const revparse = command(
    'git',
    'rev-parse',
    '--show-toplevel');

  const result = await execAsync(revparse);
  return result.stdout.trim();
}

export const readAllDirs = async (dir: string): Promise<string[]> => {
  const stat = await fs.stat(dir);
  if (!stat.isDirectory()) return [];

  const files = await fs.readdir(dir);
  const result: string[] = [dir];

  for (const file of files) {
    const fullPath = path.join(dir, file);
    const nested = await readAllDirs(fullPath);
    result.push(...nested);
  }

  return result;
}

export const readAllFiles = async (dir: string): Promise<string[]> => {
  const stat = await fs.stat(dir);
  if (stat.isFile()) return [dir];

  if (!stat.isDirectory()) return [];

  const files = await fs.readdir(dir);
  const result: string[] = [];

  for (const file of files) {
    const fullPath = path.join(dir, file);
    const nested = await readAllFiles(fullPath);
    result.push(...nested);
  }

  return result;
}

export function write(message: string, ...optionalParams: any[]): void {
  if (process.env.DEBUG) {
    if (optionalParams.length > 0) {
      console.log(message, optionalParams);
    } else {
      console.log(message);
    }
  }
};
