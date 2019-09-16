// https://github.com/angular/material.angular.io/blob/master/src/app/shared/theme-picker/theme-picker.spec.ts

import { async, TestBed } from '@angular/core/testing';

import { ThemePickerComponent } from './theme-picker.component';
import { ThemePickerModule } from '../../theme-picker.module';
// import { DocsAppTestingModule } from '../../testing/testing-module';

describe('ThemePicker', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      // imports: [ThemePickerModule, DocsAppTestingModule],
      imports: [ThemePickerModule],
    }).compileComponents();
  }));

  it('should install theme based on name', () => {
    const fixture = TestBed.createComponent(ThemePickerComponent);
    const component = fixture.componentInstance;
    const name = 'pink-bluegrey';
    spyOn(component.styleManager, 'setStyle');
    component.installTheme(name);
    expect(component.styleManager.setStyle).toHaveBeenCalled();
    expect(component.styleManager.setStyle).toHaveBeenCalledWith('theme', `assets/${name}.css`);
  });
});
