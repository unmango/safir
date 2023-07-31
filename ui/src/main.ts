import { bootstrapApplication } from '@angular/platform-browser';
import { provideStore } from "@ngrx/store";
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';

bootstrapApplication(AppComponent, {
  providers: [
    provideStore({}, {}),
    environment.providers,
  ],
}).catch((err) => console.error(err));
