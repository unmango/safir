import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from "@angular/common";
import { SidebarComponent } from './sidebar/sidebar.component';
import { HomeComponent } from "./home/home.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, SidebarComponent, HomeComponent],
  template: `
    <div class="h-screen flex flex-row bg-gradient-to-tr from-blue-950 to-black">
      <app-sidebar class="h-screen w-1/6" />
      <app-home class="w-full" />
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  title = 'safir';
}
