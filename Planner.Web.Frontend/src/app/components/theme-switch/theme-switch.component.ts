import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';

export const THEME_IS_DARK = 'planner_theme_is_dark';

@Component({
  selector: 'app-theme-switch',
  templateUrl: './theme-switch.component.html',
  styleUrls: ['./theme-switch.component.scss']
})
export class ThemeSwitchComponent implements OnInit {

  private static readonly DARK_THEME_CLASS = 'dark-theme';

  constructor(@Inject(DOCUMENT) private document: Document) {
  }

  ngOnInit(): void {
    const dark = localStorage.getItem(THEME_IS_DARK)?.toLowerCase() === 'true';

    if (this.isDark !== dark) {
      this.switchTheme();
    }
  }

  public get isDark(): boolean {
    return this.document.documentElement.classList.contains(ThemeSwitchComponent.DARK_THEME_CLASS);
  }

  public switchTheme(): void {
    if (this.isDark) {
      this.selectLightTheme();
    } else {
      this.selectDarkTheme();
    }

    // TODO: save this.isDark to local storage
    localStorage.setItem(THEME_IS_DARK, this.isDark.toString());
  }

  public selectDarkTheme(): void {
    this.document.documentElement.classList.add(ThemeSwitchComponent.DARK_THEME_CLASS);

  }

  public selectLightTheme(): void {
    this.document.documentElement.classList.remove(ThemeSwitchComponent.DARK_THEME_CLASS);
  }

}
