import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="h-full bg-black/60 border border-blue-200">
      <div class="p-2">
        <h2 class="text-white text-2xl font-bold">Safir</h2>
      </div>
      <ul class="p-2 flex flex-col gap-2">
        <li class="p-1" *ngFor="let item of items">
          <span class="text-blue-50 drop-shadow-glow">{{ item }}</span>
        </li>
      </ul>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
  items = ['Music', 'Test'];
}
