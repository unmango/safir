import { MediaClient } from "@unmango/safir-grpc-client/dist/manager";
import { MediaItem } from "@unmango/safir-protos/dist/manager";
import { from, Observable } from "rxjs";

class ProxyMediaItem extends MediaItem {

  constructor(
    private host: string,
    private path: string,
  ) {
    super();
  }

  getHost(): string {
    return this.host;
  }

  setHost(value: string): MediaItem {
    this.host = value;
    return this;
  }

  getPath(): string {
    return this.path;
  }
  
  setPath(value: string): MediaItem {
    this.path = value;
    return this;
  }

}

export class MockMediaClient extends MediaClient {
  
  public proxyItems: MediaItem[] = [
    new ProxyMediaItem('proxy', 'test.txt'),
    new ProxyMediaItem('proxy', 'music.mp3'),
  ];

  public list(): Observable<MediaItem> {
    return from(this.proxyItems);
  }

  public listAsync(): Promise<MediaItem[]> {
    return Promise.resolve(this.proxyItems);
  }

}
