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
  on(FilesActions.loadFilesSuccess, (state, res): State => ({ ...state, files: res.files })),
  on(FilesActions.loadFilesFailure, (state, err): State => ({ ...state, error: err.error })),
);

export const filesFeature = createFeature({
  name: 'files',
  reducer,
});
