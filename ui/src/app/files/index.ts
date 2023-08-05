import { filesFeature } from "./files.reducer";

export { filesFeatureKey, State, reducer, filesFeature } from './files.reducer';
export { FilesService } from './files.service';

export * as filesEffects from './files.effects';

export const {
  selectError,
  selectFiles,
  selectLoading,
} = filesFeature;
