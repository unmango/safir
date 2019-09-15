import { Component } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

import { State, selectNavCollapsed, selectNavItems } from '@app/reducers';
import { toggle } from '@app/actions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  public title = 'Safir';

  public isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset).pipe(
      map(result => result.matches),
      shareReplay()
    );

  public collapsed$ = this.store.select(selectNavCollapsed);
  public navItems$ = this.store.select(selectNavItems);

  constructor(
    private breakpointObserver: BreakpointObserver,
    private store: Store<State>
  ) { }

  public onNavToggle(): void {
    this.store.dispatch(toggle());
  }

}
