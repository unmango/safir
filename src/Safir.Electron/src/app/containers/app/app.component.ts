import { Component, ViewChild, ElementRef } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  @ViewChild('header', { static: true })
  header: ElementRef;

  public title = 'Safir';

  public isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset).pipe(
      map(result => result.matches),
      shareReplay()
    );

  public get headerHeight(): number {
    return this.header
      && this.header.nativeElement
      && this.header.nativeElement.offsetHeight
      || 0;
  }

  constructor(
    private breakpointObserver: BreakpointObserver
  ) { }

}
