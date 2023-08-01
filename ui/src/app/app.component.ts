import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FilesService } from './files/files.service';
import { AsyncPipe, NgForOf } from "@angular/common";
import { map } from "rxjs";

@Component({
  selector: 'app-root',
  template: `
    <div class="h-screen flex flex-row bg-gradient-to-tr from-blue-950 to-black">
      <app-sidebar />
      <div class="h-screen w-1/3 flex flex-col gap-2 bg-red-500">
        <div class="h-20 w-40 bg-green-700 flex flex-col" *ngFor="let file of files$ | async">
          <span>Name: {{ file.name }}</span>
          <span>Path: {{ file.fullPath }}</span>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [SidebarComponent, NgForOf, AsyncPipe],
})
export class AppComponent {
  title = 'safir';
  files$ = this.files.list().pipe(map(x => x.files));

  constructor(private files: FilesService) {}
}
