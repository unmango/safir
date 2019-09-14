import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer,
  Action
} from '@ngrx/store';

import { environment } from '../../environments/environment';
import * as fromNav from './nav.reducer';

export interface State {
  [fromNav.navFeatureKey]: fromNav.State;
}

export const reducers: ActionReducerMap<State> = {
  [fromNav.navFeatureKey]: fromNav.reducer,
};

export function logger(reducer: ActionReducer<State>): ActionReducer<State> {
  return (state: State, action: Action): State => {
    const result = reducer(state, action);
    console.groupCollapsed(action.type);
    console.log('prev state', state);
    console.log('action', action);
    console.log('next state', result);
    console.groupEnd();

    return result;
  };
}

export const metaReducers: MetaReducer<State>[] = !environment.production
  ? [logger]
  : [];
