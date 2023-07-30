import {ChangeDetectionStrategy, Component} from '@angular/core';

@Component({
  selector: 'app-sidebar',
  template: `
    <div class="h-screen w-1/6 bg-gray-950 border border-amber-200">
      <div class="p-2">
        <h2 class="text-white text-2xl font-bold">Safir</h2>
      </div>
      <ul class="p-2 flex flex-col gap-2">
        <li class="p-1" *ngFor="let item of items">
          <span class="text-amber-50 shadow-sm shadow-white">{{ item }}</span>
        </li>
      </ul>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarComponent {
  items = ['Music', 'Test']
}
