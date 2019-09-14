import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';

import { environment } from '../../environments/environment';
import * as fromNav from './nav.reducer';

export interface State {
  [fromNav.navFeatureKey]: fromNav.State;
}

export const reducers: ActionReducerMap<State> = {
  [fromNav.navFeatureKey]: fromNav.reducer,
};


export const metaReducers: MetaReducer<State>[] = !environment.production
  ? []
  : [];
