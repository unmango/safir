#!/usr/bin/env ts-node

import * as fs from 'fs/promises';
import * as fsSync from 'fs';
import * as path from 'path';
import * as googleapis from 'google-proto-files';
import * as util from './util';

const { execAsync, globAsync, write } = util;

(async function () {
  const cwd = process.cwd();
  write('cwd: ' + cwd);

  let gitRoot: string;
  if (process.env.GIT_ROOT) {
    gitRoot = process.env.GIT_ROOT;
  } else {
    gitRoot = await util.gitRootAsync();
  }

  const indir = path.join(gitRoot, 'src', 'protos');
  write('indir: ' + indir);

  const cachedir = path.join(cwd, '.cache');
  write('cachedir: ' + cachedir);

  const gendir = path.join(cwd, 'generated');
  write('gendir: ' + gendir);

  const outdir = path.join(cwd, 'dist');
  write('outdir: ' + outdir);

  write('Checking if cachedir exists');
  if (fsSync.existsSync(cachedir)) {
    write('Removing cachedir');
    await fs.rm(cachedir, { recursive: true });
  }

  write('Checking if gendir exists');
  if (fsSync.existsSync(gendir)) {
    write('Removing gendir');
    await fs.rm(gendir, { recursive: true });
  }

  write('Checking if outdir exists');
  if (fsSync.existsSync(outdir)) {
    write('Removing outdir');
    await fs.rm(outdir, { recursive: true });
  }

  write('Collecting vendored protos');
  const googleapiPath = path.join(cachedir, 'google', 'api');
  await fs.mkdir(googleapiPath, { recursive: true });

  write('Copying annotations proto');
  const annotationsPath = googleapis.getProtoPath('api', 'annotations.proto');
  await fs.copyFile(annotationsPath, path.join(googleapiPath, 'annotations.proto'));

  write('Copying http proto');
  const httpPath = googleapis.getProtoPath('api', 'http.proto');
  await fs.copyFile(httpPath, path.join(googleapiPath, 'http.proto'));

  const jsOutOptions = [
    'import_style=commonjs',
  ].join(',') + ':' + gendir;
  write('jsOutOptions: ' + jsOutOptions);

  const grpcWebOutOptions = [
    'import_style=typescript',
    'mode=grpcwebtext'
  ].join(',') + ':' + gendir;
  write('grpcWebOutOptions: ' + grpcWebOutOptions);

  var annoTest = await googleapis.load('')

  write('Collecting input files');
  const globbedProtoPath = path.join(indir, '**/*.proto');
  const srcFiles = await globAsync(globbedProtoPath, { follow: false });
  // TODO: Find the supposed package w/ pre-generated code for googleapis
  const globbedCachePath = path.join(cachedir, '**/*.proto');
  const cacheFiles = await globAsync(globbedCachePath, { follow: false });
  const files = [...srcFiles, ...cacheFiles];
  write('Files:\n  ' + files.join('\n  '));

  const protocCommand = [
    'protoc',
    '-I=' + indir,
    '-I=' + cachedir,
    ...files,
    '--js_out=' + jsOutOptions,
    '--grpc-web_out=' + grpcWebOutOptions
  ].join(' ');
  write('protocCommand: ' + protocCommand);

  write('Checking if gendir exists');
  if (!fsSync.existsSync(gendir)) {
    write('Creating gendir');
    await fs.mkdir(gendir);
  }

  write('Executing protocCommand');
  await execAsync(protocCommand);

  write('Collecting directories in gendir');
  const gendirs = await util.readAllDirs(gendir);
  write('Output directories:\n  ' + gendirs.join('\n  '));

  for (const dir of gendirs) {
    write('Reading files from ' + dir);
    const files = await fs.readdir(dir);

    const dirs: string[] = [];
    const code: string[] = [];

    for (const file of files) {
      const fullPath = path.join(dir, file);
      const stat = await fs.stat(fullPath);

      if (stat.isFile()) {
        code.push(file);
      } else {
        dirs.push(file);
      }
    }

    const indexFile = path.join(dir, 'index.ts');

    if (dirs.length > 0) {
      const modules: string[] = [];

      for (const nDir of dirs) {
        const fullDir = path.join(dir, nDir);
        const nFiles = await fs.readdir(fullDir);

        if (nFiles.length <= 0) continue;

        modules.push(getModule(nDir));
        write('Writing module imports to ' + indexFile);
        await fs.appendFile(indexFile, asImport(nDir, true) + '\n');
      }

      const toExport = modules.join(', ');
      write('Writing module exports to ' + indexFile);
      await fs.appendFile(indexFile, `export { ${toExport} };`);
    }

    if (code.length > 0) {
      const tsContent = code.map(x => asExport(x)).join('\n') + '\n';
      write('Writing code exports to ' + indexFile);
      await fs.appendFile(indexFile, tsContent);
    }
  }

  try {
    write('Executing tsc');
    await execAsync('tsc');
  } catch (err) {
    console.error(err);
  }

  write('Copying type definitions');
  const toCopy = await globAsync(path.join(gendir, '**', '*.{d.ts,js}'));
  write('Files to copy:\n' + toCopy.join('\n  '));
  for (const file of toCopy) {
    const newFile = file.replace(gendir, outdir);
    write('Copy:\n  ' + file + '\nto\n  ' + newFile);
    await fs.copyFile(file, newFile);
  }
}());

function getModule(file: string): string {
  return file
    .replace('.d', '')
    .replace('.ts', '')
    .replace('.js', '');
}

function asExport(file: string, asModule = false): string {
  const module = getModule(file);

  return asModule
    ? `export * as ${module} from './${module}';`
    : `export * from './${module}';`;
};

function asImport(file: string, asModule = false): string {
  const module = getModule(file);

  return asModule
    ? `import * as ${module} from './${module}';`
    : `import * from './${module}';`;
};
