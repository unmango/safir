import { FileSystemClient } from "@unmango/safir-grpc-client/dist/agent";
import { FileSystemEntry } from "@unmango/safir-protos/dist/agent";
import { from, Observable } from "rxjs";

class ProxyEntry extends FileSystemEntry {

  constructor(private path: string) {
    super();
  }

  getPath(): string {
    return this.path;
  }

  setPath(value: string): FileSystemEntry {
    this.path = value;
    return this;
  }

}

export class MockFileSystemClient extends FileSystemClient {

  public proxyFiles: FileSystemEntry[] = [
    new ProxyEntry('test.txt'),
    new ProxyEntry('music.mp3'),
  ];
  
  public listFiles(): Observable<FileSystemEntry> {
    return from(this.proxyFiles);
  }

  public listFilesAsync(): Promise<FileSystemEntry[]> {
    return Promise.resolve(this.proxyFiles);
  }

}
