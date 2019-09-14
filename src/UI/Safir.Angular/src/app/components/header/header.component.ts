import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Input() isHandset: boolean;

  @Input() title: string;

  @Output() navToggled = new EventEmitter<void>();

  constructor() { }

  ngOnInit() { }

  public onNavToggle() {
    this.navToggled.emit();
  }

}
