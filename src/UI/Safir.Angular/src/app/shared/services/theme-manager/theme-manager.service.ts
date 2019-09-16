import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import { SiteTheme } from '@app/shared/models';
import { State } from './theme-manager.reducer';

@Injectable()
export class ThemeManagerService {

  public available$: Observable<SiteTheme[]> = this.store
    .select(state => state.available);

  public current$: Observable<SiteTheme | null> = this.store
    .select(state => state.current);

  constructor(private store: Store<State>) { }

}
