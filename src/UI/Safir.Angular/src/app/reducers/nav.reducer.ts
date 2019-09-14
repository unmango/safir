import { Action, createReducer, on } from '@ngrx/store';

import { collapse } from '../actions';

export const navFeatureKey = 'nav';

export interface State {
  mode: 'text' | 'icon';
}

export const initialState: State = {
  mode: 'text'
};

const navReducer = createReducer(
  initialState,
  on(collapse, state => ({ ...state, mode: 'icon' }))
);

export function reducer(state: State | undefined, action: Action) {
  return navReducer(state, action);
}
