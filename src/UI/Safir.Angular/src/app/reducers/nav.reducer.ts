import { Action, createReducer, on } from '@ngrx/store';

import { NavItem } from '@app/models';
import { collapse, expand } from '../actions';

export const navFeatureKey = 'nav';

export interface State {
  collapsed: boolean;
  items: NavItem[];
}

export const initialState: State = {
  collapsed: false,
  items: [
    { href: '/', name: 'Home' },
    { href: '/library', name: 'Library' }
  ]
};

const navReducer = createReducer(
  initialState,
  on(collapse, state => ({ ...state, collapsed: true })),
  on(expand, state => ({ ...state, collapsed: false }))
);

export function reducer(state: State | undefined, action: Action) {
  return navReducer(state, action);
}
