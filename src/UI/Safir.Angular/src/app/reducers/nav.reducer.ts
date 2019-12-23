import { Action, createReducer, on } from '@ngrx/store';

import { NavItem } from '@app/models';
import { collapse, expand } from '../actions';

export const navFeatureKey = 'nav';

export interface State {
  collapsed: boolean;
  availableItems: NavItem[];
  items: NavItem[];
}

export const initialState: State = {
  collapsed: false,
  availableItems: [],
  items: [
    { href: '/', name: 'Home', icon: 'home' },
    { href: '/library', name: 'Library', icon: 'library_music' }
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
