import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, map, of } from 'rxjs';
import { FilesActions } from './files.actions';
import { FilesService } from './files.service';

export const loadFiles = createEffect(
  (actions$ = inject(Actions), service = inject(FilesService)) => {
    return actions$.pipe(
      ofType(FilesActions.loadFiles),
      exhaustMap(() =>
        service.list().pipe(
          map(FilesActions.loadFilesSuccess),
          catchError((err) => of(FilesActions.loadFilesFailure({ error: err.toString() }))),
        ),
      ),
    );
  },
  { functional: true },
);

export const discoverFile = createEffect(
  (actions$ = inject(Actions), service = inject(FilesService)) => {
    return actions$.pipe(
      ofType(FilesActions.discoverFile),
      exhaustMap((req) =>
        service.discover(req).pipe(
          map(FilesActions.discoverFileSuccess),
          catchError((err) => of(FilesActions.discoverFileError({ error: err.toString() }))),
        ),
      ),
    );
  },
  { functional: true },
);
