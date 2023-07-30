import { StoreDevtoolsModule } from '@ngrx/store-devtools';

export const environment = {
  production: false,
  imports: [StoreDevtoolsModule.instrument({ maxAge: 25 })],
};
