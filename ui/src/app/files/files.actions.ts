import { createActionGroup, emptyProps, props } from '@ngrx/store';
import {
  FilesServiceDiscoverRequest,
  FilesServiceDiscoverResponse,
  FilesServiceListResponse,
} from 'src/gen/safir/v1alpha1/files_pb';

export const FilesActions = createActionGroup({
  source: 'Files',
  events: {
    'Load Files': emptyProps(),
    'Load Files Success': props<FilesServiceListResponse>(),
    'Load Files Failure': props<{ error: string }>(),
    'Discover File': props<FilesServiceDiscoverRequest>(),
    'Discover File Success': props<FilesServiceDiscoverResponse>(),
    'Discover File Error': props<{ error: string }>(),
  }
});
