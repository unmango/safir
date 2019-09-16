import { Action, createReducer, createSelector, on } from '@ngrx/store';
import { SiteTheme } from '@app/shared/models';
import { installTheme } from './theme-manager.actions';

export const themeManagerFeatureKey = 'themeManager';

const staticThemes: SiteTheme[] = [
  {
    primary: '#673AB7',
    accent: '#FFC107',
    name: 'deeppurple-amber',
    isDark: false,
  },
  {
    primary: '#3F51B5',
    accent: '#E91E63',
    name: 'indigo-pink',
    isDark: false,
    isDefault: true,
  },
  {
    primary: '#E91E63',
    accent: '#607D8B',
    name: 'pink-bluegrey',
    isDark: true,
  },
  {
    primary: '#9C27B0',
    accent: '#4CAF50',
    name: 'purple-green',
    isDark: true,
  },
];

export interface State {
  available: SiteTheme[];
  current?: SiteTheme | null;
}

export const initialState: State = {
  available: staticThemes
};

const themeReducer = createReducer(
  initialState,
  on(installTheme, (state, { name }) => ({
    ...state,
    current: state.available.find(theme => theme.name === name)
  }))
);

export function reducer(state: State | undefined, action: Action): State {
  return themeReducer(state, action);
}

export const selectTheme = (state: { [themeManagerFeatureKey]: State }) => state[themeManagerFeatureKey];
export const selectAvailable = createSelector(selectTheme, state => state.available);
export const selectCurrent = createSelector(selectTheme, state => state.current);
