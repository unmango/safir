import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { FilesActions, selectFiles, selectLoading } from '../files';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="h-full p-3 flex flex-col gap-2">
      <h1 class="text-4xl text-white font-bold">Files</h1>
      <span *ngIf="loading()" class="text-white">Loading...</span>
      <ul *ngIf="!loading()" class="h-full p-2 border border-blue-50 flex flex-col gap-2">
        <li *ngFor="let file of files()" class="text-white border border-blue-50 p-1 hover:bg-white/10">
          {{ file.name }}
        </li>
      </ul>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent implements OnInit {
  readonly files = this.store.selectSignal(selectFiles);
  readonly loading = this.store.selectSignal(selectLoading);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(FilesActions.loadFiles());
  }
}
