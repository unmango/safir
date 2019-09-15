import { Component, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';

import { NavItem } from '@app/models';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
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
    return !this.isHandset || !this.collapsed;
  }

}
