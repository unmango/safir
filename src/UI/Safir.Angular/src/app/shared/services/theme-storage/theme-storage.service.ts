// https://github.com/angular/material.angular.io/blob/master/src/app/shared/theme-picker/theme-storage/theme-storage.ts

import { Injectable, EventEmitter } from '@angular/core';

import { SiteTheme } from '@app/shared/models';

@Injectable()
export class ThemeStorage {

  static storageKey = 'safir-theme-storage-current-name';

  onThemeUpdate: EventEmitter<SiteTheme> = new EventEmitter<SiteTheme>();

  storeTheme(theme: SiteTheme) {
    try {
      window.localStorage[ThemeStorage.storageKey] = theme.name;
    } catch { }

    this.onThemeUpdate.emit(theme);
  }

  getStoredThemeName(): string | null {
    try {
      return window.localStorage[ThemeStorage.storageKey] || null;
    } catch {
      return null;
    }
  }

  clearStorage() {
    try {
      window.localStorage.removeItem(ThemeStorage.storageKey);
    } catch { }
  }

}
