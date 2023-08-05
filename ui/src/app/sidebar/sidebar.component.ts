import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  template: `
    <div class="p-2">
      <h2 class="text-white text-2xl font-bold">Safir</h2>
    </div>
    <ul class="p-2 flex flex-col gap-2">
      <li class="p-1" *ngFor="let item of items">
        <span class="text-blue-50 drop-shadow-glow">{{ item }}</span>
      </li>
    </ul>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [NgFor],
})
export class SidebarComponent {
  items = ['Music', 'Test'];
}
