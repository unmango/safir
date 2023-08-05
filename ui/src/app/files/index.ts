import { filesFeature } from './files.state';

export * as filesEffects from './files.effects';

export { filesFeature };

export const { selectError, selectFiles, selectLoading } = filesFeature;
