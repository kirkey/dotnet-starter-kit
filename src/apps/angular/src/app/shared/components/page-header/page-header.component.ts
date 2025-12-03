import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-header">
      <div class="header-content">
        <h1 class="title">{{ title }}</h1>
        @if (subtitle) {
          <p class="subtitle">{{ subtitle }}</p>
        }
      </div>
      <div class="header-actions">
        <ng-content></ng-content>
      </div>
    </div>
  `,
  styles: [`
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
      padding-bottom: 16px;
      border-bottom: 1px solid rgba(0, 0, 0, 0.12);
    }

    .header-content {
      flex: 1;
    }

    .title {
      margin: 0;
      font-size: 24px;
      font-weight: 500;
      color: var(--text-primary);
    }

    .subtitle {
      margin: 4px 0 0;
      font-size: 14px;
      color: var(--text-secondary);
    }

    .header-actions {
      display: flex;
      gap: 8px;
      align-items: center;
    }

    :host-context(.dark-theme) .page-header {
      border-bottom-color: rgba(255, 255, 255, 0.12);
    }
  `]
})
export class PageHeaderComponent {
  @Input({ required: true }) title!: string;
  @Input() subtitle?: string;
}
