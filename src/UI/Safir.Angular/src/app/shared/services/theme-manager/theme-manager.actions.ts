import { createAction, props } from '@ngrx/store';

export const installTheme = createAction(
  '[Theme] Install',
  props<{ name: string }>()
);
