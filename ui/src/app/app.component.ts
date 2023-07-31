import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SidebarComponent } from './sidebar/sidebar.component';

@Component({
  selector: 'app-root',
  template: `
    <div class="h-screen bg-gradient-to-tr from-blue-950 to-black">
      <app-sidebar />
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [SidebarComponent],
})
export class AppComponent {
  title = 'safir';
}
