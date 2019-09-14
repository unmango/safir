import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';

import { reducers, metaReducers } from './reducers';

@NgModule({
  imports: [
    StoreModule.forRoot(reducers, {
      metaReducers,
      runtimeChecks: {
        strictStateImmutability: true,
        strictActionImmutability: true
      }
    })
  ],
  exports: [StoreModule]
})
export class AppStoreModule { }
