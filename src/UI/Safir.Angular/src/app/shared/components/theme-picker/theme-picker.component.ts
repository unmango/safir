// https://github.com/angular/material.angular.io/blob/master/src/app/shared/theme-picker/theme-picker.ts

import {
  Component,
  ViewEncapsulation,
  ChangeDetectionStrategy,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { StyleManager, ThemeStorage } from '@app/shared/services';
import { SiteTheme } from '@app/shared/models';
import { ThemeManagerService } from '@app/shared/services/theme-manager/theme-manager.service';

@Component({
  selector: 'app-theme-picker',
  templateUrl: 'theme-picker.component.html',
  styleUrls: ['theme-picker.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  host: { 'aria-hidden': 'true' },
})
export class ThemePickerComponent implements OnInit, OnDestroy {

  private _queryParamSubscription = Subscription.EMPTY;
  currentTheme: SiteTheme;

  themes: SiteTheme[] = [
    {
      primary: '#673AB7',
      accent: '#FFC107',
      name: 'deeppurple-amber',
      isDark: false,
    },
    {
      primary: '#3F51B5',
      accent: '#E91E63',
      name: 'indigo-pink',
      isDark: false,
      isDefault: true,
    },
    {
      primary: '#E91E63',
      accent: '#607D8B',
      name: 'pink-bluegrey',
      isDark: true,
    },
    {
      primary: '#9C27B0',
      accent: '#4CAF50',
      name: 'purple-green',
      isDark: true,
    },
  ];

  constructor(
    public styleManager: StyleManager,
    private _themeManager: ThemeManagerService,
    private _themeStorage: ThemeStorage,
    private _activatedRoute: ActivatedRoute) {
    this.installTheme(this._themeStorage.getStoredThemeName());
  }

  ngOnInit(): void {
    this._queryParamSubscription = this._activatedRoute.queryParamMap
      .pipe(map(params => params.get('theme')), filter(Boolean))
      .subscribe((themeName: string) => this.installTheme(themeName));
  }

  ngOnDestroy(): void {
    this._queryParamSubscription.unsubscribe();
  }

  installTheme(themeName: string): void {
    const theme = this.themes.find(currentTheme => currentTheme.name === themeName);

    if (!theme) {
      return;
    }

    this.currentTheme = theme;

    if (theme.isDefault) {
      this.styleManager.removeStyle('theme');
    } else {
      this.styleManager.setStyle('theme', `assets/${theme.name}.css`);
    }

    if (this.currentTheme) {
      this._themeStorage.storeTheme(this.currentTheme);
    }
  }

}
