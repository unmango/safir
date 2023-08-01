import { provideStoreDevtools } from "@ngrx/store-devtools";

export const environment = {
  production: false,
  providers: [
    provideStoreDevtools({ maxAge: 25 }),
  ],
  service: {
    baseUrl: 'http://localhost:5020'
  }
};
