import { filesFeature } from './files.state';

export * as filesEffects from './files.effects';
export * from './files.actions';

export { filesFeature };

export const { selectError, selectFiles, selectLoading } = filesFeature;
