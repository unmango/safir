import { Injectable } from '@angular/core';
import { createPromiseClient } from '@bufbuild/connect';
import { createGrpcWebTransport } from '@bufbuild/connect-web';
import { from, Observable } from 'rxjs';
import { environment } from '../../environments';
import * as connect from 'src/gen/safir/v1alpha1/files_connect';
import {
  FilesServiceDiscoverRequest,
  FilesServiceDiscoverResponse,
  FilesServiceListResponse,
} from 'src/gen/safir/v1alpha1/files_pb';

@Injectable({
  providedIn: 'root',
})
export class FilesService {
  transport = createGrpcWebTransport({
    baseUrl: environment.service.baseUrl,
    useBinaryFormat: true,
  });

  client = createPromiseClient(connect.FilesService, this.transport);

  discover(req: FilesServiceDiscoverRequest): Observable<FilesServiceDiscoverResponse> {
    return from(this.client.discover(req));
  }

  list(): Observable<FilesServiceListResponse> {
    return from(this.client.list({}));
  }
}
