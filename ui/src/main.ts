import { bootstrapApplication } from '@angular/platform-browser';
import { provideStore } from "@ngrx/store";
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';
import { provideEffects } from '@ngrx/effects';

bootstrapApplication(AppComponent, {
  providers: [
    provideStore({}, {}),
    environment.providers,
    provideEffects()
],
}).catch((err) => console.error(err));
