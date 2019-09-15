import { Component, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

import { NavItem } from '@app/models';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
  animations: [
    trigger('slideInOut', [
      state('collapsed', style({
        // transform: 'translate3d(0,0,0)'
        width: '*'
      })),
      state('opened', style({
        // transform: 'translate3d(100%,0,0)'
        width: '200px'
      })),
      transition('collapsed => opened', animate('400ms ease-in-out')),
      transition('opened => collapsed', animate('400ms ease-in-out'))
    ]),
    trigger('fadeInOut', [
      transition(':enter', [
        style({
          transform: 'translateX(-100%)',
          opacity: 0
        }),
        animate(400, style({
          transform: 'translateX(0)',
          opacity: 1
        }))
      ]),
      transition(':leave', [
        animate(400, style({
          transform: 'translateX(0)',
          opacity: 0
        }))
      ])
    ])
  ]
})
export class NavComponent {

  @Input() isHandset: boolean;
  @Input() topGap: number;
  @Input() collapsed: boolean;
  @Input() items: NavItem[];

  @Output() toggled = new EventEmitter<void>();

  public get mode(): 'over' | 'side' {
    return this.isHandset ? 'over' : 'side';
  }

  public get opened(): boolean {
    return !this.collapsed;
  }

  public get state(): string {
    return this.collapsed ? 'collapsed' : 'opened';
  }

  public toggle(): void {
    this.toggled.emit();
  }

}
