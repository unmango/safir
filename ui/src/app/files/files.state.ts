import { createFeature, createReducer, on } from '@ngrx/store';
import { FilesActions } from './files.actions';
import { File } from 'src/gen/safir/v1alpha1/files_pb';

export interface State {
  files: ReadonlyArray<File>;
  error: string | null;
  loading: boolean;
}

export const initialState: State = {
  files: [],
  error: null,
  loading: false,
};

export const reducer = createReducer(
  initialState,
  on(FilesActions.loadFiles, (state): State => ({ ...state, loading: true })),
  on(
    FilesActions.loadFilesSuccess,
    (state, res): State => ({ ...state, files: res.files, loading: false, error: null }),
  ),
  on(FilesActions.loadFilesFailure, (state, err): State => ({ ...state, error: err.error, loading: false })),
);

export const filesFeature = createFeature({
  name: 'files',
  reducer,
});
