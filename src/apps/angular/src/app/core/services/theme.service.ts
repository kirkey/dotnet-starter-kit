import { Injectable, signal, effect, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { OverlayContainer } from '@angular/cdk/overlay';

export interface ThemeSettings {
  isDarkMode: boolean;
  primaryColor: string;
  accentColor: string;
  borderRadius: number;
}

const THEME_STORAGE_KEY = 'theme_settings';

const DEFAULT_SETTINGS: ThemeSettings = {
  isDarkMode: false,
  primaryColor: '#594ae2',
  accentColor: '#ff4081',
  borderRadius: 4
};

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private platformId = inject(PLATFORM_ID);
  private overlayContainer = inject(OverlayContainer);
  
  private _settings = signal<ThemeSettings>(this.loadSettings());
  
  readonly settings = this._settings.asReadonly();
  readonly isDarkMode = () => this._settings().isDarkMode;

  constructor() {
    // Effect to apply theme changes
    effect(() => {
      const settings = this._settings();
      this.applyTheme(settings);
      this.saveSettings(settings);
    });
  }

  toggleDarkMode(): void {
    this._settings.update(settings => ({
      ...settings,
      isDarkMode: !settings.isDarkMode
    }));
  }

  setPrimaryColor(color: string): void {
    this._settings.update(settings => ({
      ...settings,
      primaryColor: color
    }));
  }

  setAccentColor(color: string): void {
    this._settings.update(settings => ({
      ...settings,
      accentColor: color
    }));
  }

  setBorderRadius(radius: number): void {
    this._settings.update(settings => ({
      ...settings,
      borderRadius: radius
    }));
  }

  private applyTheme(settings: ThemeSettings): void {
    if (!isPlatformBrowser(this.platformId)) return;

    const body = document.body;
    const overlayContainerEl = this.overlayContainer.getContainerElement();

    // Apply dark mode
    if (settings.isDarkMode) {
      body.classList.add('dark-theme');
      overlayContainerEl.classList.add('dark-theme');
    } else {
      body.classList.remove('dark-theme');
      overlayContainerEl.classList.remove('dark-theme');
    }

    // Apply CSS variables
    document.documentElement.style.setProperty('--primary-color', settings.primaryColor);
    document.documentElement.style.setProperty('--accent-color', settings.accentColor);
    document.documentElement.style.setProperty('--border-radius', `${settings.borderRadius}px`);
  }

  private loadSettings(): ThemeSettings {
    if (!isPlatformBrowser(this.platformId)) {
      return DEFAULT_SETTINGS;
    }

    const stored = localStorage.getItem(THEME_STORAGE_KEY);
    if (stored) {
      try {
        return { ...DEFAULT_SETTINGS, ...JSON.parse(stored) };
      } catch {
        return DEFAULT_SETTINGS;
      }
    }
    return DEFAULT_SETTINGS;
  }

  private saveSettings(settings: ThemeSettings): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(THEME_STORAGE_KEY, JSON.stringify(settings));
    }
  }
}
