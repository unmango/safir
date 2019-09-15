import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer,
  Action
} from '@ngrx/store';
import * as fromRouter from '@ngrx/router-store';

import { environment } from '../../environments/environment';
import * as fromNav from './nav.reducer';

export interface State {
  router: fromRouter.RouterReducerState<any>;
  [fromNav.navFeatureKey]: fromNav.State;
}

export const reducers: ActionReducerMap<State> = {
  router: fromRouter.routerReducer,
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

export const selectRouter = createFeatureSelector<State, fromRouter.RouterReducerState<any>>('router');
export const {
  selectQueryParams,    // select the current route query params
  selectQueryParam,     // factory function to select a query param
  selectRouteParams,    // select the current route params
  selectRouteParam,     // factory function to select a route param
  selectRouteData,      // select the current route data
  selectUrl,            // select the current url
} = fromRouter.getSelectors(selectRouter);

export const selectNav = createFeatureSelector<State, fromNav.State>(fromNav.navFeatureKey);
export const selectNavCollapsed = createSelector(selectNav, state => state.collapsed);
export const selectNavItems = createSelector(selectNav, state => state.items);
