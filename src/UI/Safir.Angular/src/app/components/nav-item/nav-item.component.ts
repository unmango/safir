import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';

import { NavItem } from '@app/models';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-nav-item',
  templateUrl: './nav-item.component.html',
  styleUrls: ['./nav-item.component.scss']
})
export class NavItemComponent implements OnInit {

  @Input() model: NavItem;

  constructor() { }

  ngOnInit() {
  }

}
