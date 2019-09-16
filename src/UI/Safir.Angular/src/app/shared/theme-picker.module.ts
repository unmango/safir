// https://github.com/angular/material.angular.io/blob/master/src/app/shared/theme-picker/theme-picker.ts

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule,
  MatIconModule,
  MatMenuModule,
  MatGridListModule,
  MatTooltipModule
} from '@angular/material';

import { ThemePickerComponent } from './components';
import { StyleManager, ThemeStorage, ThemeManagerService } from './services';

@NgModule({
  declarations: [ThemePickerComponent],
  imports: [
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatGridListModule,
    MatTooltipModule,
    CommonModule
  ],
  exports: [ThemePickerComponent],
  providers: [StyleManager, ThemeStorage, ThemeManagerService],
})
export class ThemePickerModule { }
