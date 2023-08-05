import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { FilesEffects } from './files.effects';

describe('FilesEffects', () => {
  let actions$: Observable<any>;
  let effects: FilesEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        FilesEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.inject(FilesEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
