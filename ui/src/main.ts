import { bootstrapApplication } from '@angular/platform-browser';
import { provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { environment } from "./environments";
import { AppComponent } from './app/app.component';
import { filesFeature, filesEffects } from './app/files';

bootstrapApplication(AppComponent, {
  providers: [
    provideStore(),
    provideState(filesFeature),
    provideEffects(filesEffects),
    environment.providers
  ],
}).catch((err) => console.error(err));
