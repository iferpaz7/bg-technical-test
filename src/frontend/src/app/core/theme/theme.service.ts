import { Injectable, effect, inject, signal } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { ThemeClass } from './types';
import { THEME_CLASSES } from './theme-options';

const THEME_COOKIE_NAME = 'theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly _cookieService = inject(CookieService);
  private readonly _themeCookie = this._cookieService.get(THEME_COOKIE_NAME);
  private readonly _theme = signal<ThemeClass>(this.#getCurrentTheme());

  theme = this._theme.asReadonly();

  constructor() {
    effect(() => {
      this.#saveThemeCookie();
    });
  }

  changeTheme(newTheme: ThemeClass) {
    this._theme.set(newTheme);
  }

  #getCurrentTheme(): ThemeClass {
    return this.#validateThemeCookie(this._themeCookie)
      ? this._themeCookie
      : 'rose-light';
  }

  #validateThemeCookie(theme: string): theme is ThemeClass {
    return THEME_CLASSES.includes(theme as ThemeClass);
  }

  #saveThemeCookie() {
    this._cookieService.set(THEME_COOKIE_NAME, this._theme());
  }
}
