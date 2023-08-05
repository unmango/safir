import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import { CommonModule } from '@angular/common';
import { Store } from "@ngrx/store";
import { selectFiles } from "../files";
import { FilesActions } from "../files/files.actions";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <ul class="bg-blue-600">
      <li *ngFor="let file of files()">{{ file.name }}</li>
    </ul>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {
  readonly files = this.store.selectSignal(selectFiles);

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.store.dispatch(FilesActions.loadFiles());
  }
}
