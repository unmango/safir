import {NgModule, isDevMode} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {StoreModule} from '@ngrx/store';
import {AppComponent} from './app.component';
import {SidebarComponent} from './sidebar/sidebar.component';
import {environment} from "../environments/environment";

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule,
    StoreModule.forRoot({}, {}),
    environment.imports,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
