import { bootstrapApplication } from '@angular/platform-browser';
import { provideState, provideStore } from "@ngrx/store";
import { provideEffects } from '@ngrx/effects';
import { environment } from "./environments";
import { AppComponent } from './app/app.component';
import { reducers } from "./app/reducers";

bootstrapApplication(AppComponent, {
  providers: [
    provideStore(reducers, {}),
    provideState(reducers),
    provideEffects(),
    environment.providers
],
}).catch((err) => console.error(err));
