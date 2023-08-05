import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { FilesServiceListResponse } from 'src/gen/safir/v1alpha1/files_pb';

export const FilesActions = createActionGroup({
  source: 'Files',
  events: {
    'Load Files': emptyProps(),
    'Load Files Success': props<FilesServiceListResponse>(),
    'Load Files Failure': props<{ error: string }>(),
  }
});
