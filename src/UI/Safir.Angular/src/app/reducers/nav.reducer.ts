import { Action, createReducer, on } from '@ngrx/store';

import { collapse, expand } from '../actions';

export const navFeatureKey = 'nav';

export interface State {
  collapsed: boolean;
}

export const initialState: State = {
  collapsed: false
};

const navReducer = createReducer(
  initialState,
  on(collapse, state => ({ ...state, collapsed: true })),
  on(expand, state => ({ ...state, collapsed: false }))
);

export function reducer(state: State | undefined, action: Action) {
  return navReducer(state, action);
}
