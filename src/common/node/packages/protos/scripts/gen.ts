export const exportAs = (from: string, as: string): string => {
  return `export * as ${as} from '${from}';`;
}

export const exportAll = (from: string, ...exports: string[]): string => {
  if (exports.length <= 0) throw new Error('At least one export is required');
  return `export { ${exports.join(', ')} } from '${from}';`;
}

export const importAs = (from: string, as: string): string => {
  return `import * as ${as} from '${from}';`;
}

export const importAll = (from: string, ...imports: string[]): string => {
  if (imports.length <= 0) throw new Error('At least one import is required');
  return `import { ${imports.join(', ')} } from '${from}';`;
}

export const indexBuilder = (): IndexBuilder => new IndexBuilder();

export const lines = (statements: string[]): string => {
  return statements.map(x => x + '\n').join('');
}

export class IndexBuilder {
  private readonly imports: [string, string[]][] = [];
  private readonly importModules: [string, string][] = [];
  private readonly exports: [string, string[]][] = [];
  private readonly exportModules: [string, string][] = [];

  public build(): string {
    let result = '';
    result += lines(this.importModules.map(x => importAs(...x)));
    result += lines(this.imports.map(([from, imports]) => importAll(from, ...imports)));
    result += lines(this.exportModules.map(x => exportAs(...x)));
    result += lines(this.exports.map(([from, exports]) => exportAll(from, ...exports)));

    return result;
  }

  public exportAs(from: string, as: string): IndexBuilder {
    this.exportModules.push([from, as]);
    return this;
  }

  public exportAll(from: string, ...exports: string[]): IndexBuilder {
    this.exports.push([from, exports]);
    return this;
  }

  public importAs(from: string, as: string): IndexBuilder {
    this.importModules.push([from, as]);
    return this;
  }

  public importAll(from: string, ...imports: string[]): IndexBuilder {
    this.imports.push([from, imports]);
    return this;
  }

  public static create(): IndexBuilder { return new IndexBuilder(); }
}
