import { Directive, Input, TemplateRef, ViewContainerRef, inject, OnInit, OnDestroy } from '@angular/core';
import { PermissionService } from '../services/permission.service';
import { Subject, takeUntil } from 'rxjs';

/**
 * Structural directive that conditionally shows/hides elements based on user permissions.
 * 
 * Usage:
 * <button *hasPermission="{ action: 'Create', resource: 'Products' }">Create Product</button>
 * 
 * Or with inverse logic:
 * <div *hasPermission="{ action: 'View', resource: 'Users', inverse: true }">
 *   No permission to view users
 * </div>
 */
@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private permissionService = inject(PermissionService);
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private destroy$ = new Subject<void>();
  
  private hasView = false;
  private permissionConfig?: { action: string; resource: string; inverse?: boolean };
  
  @Input() set hasPermission(config: { action: string; resource: string; inverse?: boolean }) {
    this.permissionConfig = config;
    this.updateView();
  }

  ngOnInit(): void {
    // Re-evaluate permission when permissions change
    this.permissionService.userPermissions().length; // Access to trigger reactivity
    this.updateView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    if (!this.permissionConfig) {
      return;
    }

    const { action, resource, inverse = false } = this.permissionConfig;
    const hasPermission = this.permissionService.hasPermission(action, resource);
    const shouldShow = inverse ? !hasPermission : hasPermission;

    if (shouldShow && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!shouldShow && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}

/**
 * Structural directive that shows content if user has ANY of the specified permissions.
 * 
 * Usage:
 * <div *hasAnyPermission="[
 *   { action: 'View', resource: 'Dashboard' },
 *   { action: 'View', resource: 'Analytics' }
 * ]">
 *   Dashboard content
 * </div>
 */
@Directive({
  selector: '[hasAnyPermission]',
  standalone: true
})
export class HasAnyPermissionDirective implements OnInit, OnDestroy {
  private permissionService = inject(PermissionService);
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private destroy$ = new Subject<void>();
  
  private hasView = false;
  private permissions?: Array<{ action: string; resource: string }>;
  
  @Input() set hasAnyPermission(permissions: Array<{ action: string; resource: string }>) {
    this.permissions = permissions;
    this.updateView();
  }

  ngOnInit(): void {
    this.updateView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    if (!this.permissions || this.permissions.length === 0) {
      return;
    }

    const hasPermission = this.permissions.some(p => 
      this.permissionService.hasPermission(p.action, p.resource)
    );

    if (hasPermission && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!hasPermission && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}

/**
 * Structural directive that shows content if user has ALL of the specified permissions.
 * 
 * Usage:
 * <div *hasAllPermissions="[
 *   { action: 'View', resource: 'Users' },
 *   { action: 'Update', resource: 'Users' }
 * ]">
 *   Edit user content
 * </div>
 */
@Directive({
  selector: '[hasAllPermissions]',
  standalone: true
})
export class HasAllPermissionsDirective implements OnInit, OnDestroy {
  private permissionService = inject(PermissionService);
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private destroy$ = new Subject<void>();
  
  private hasView = false;
  private permissions?: Array<{ action: string; resource: string }>;
  
  @Input() set hasAllPermissions(permissions: Array<{ action: string; resource: string }>) {
    this.permissions = permissions;
    this.updateView();
  }

  ngOnInit(): void {
    this.updateView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    if (!this.permissions || this.permissions.length === 0) {
      return;
    }

    const hasAllPermissions = this.permissions.every(p => 
      this.permissionService.hasPermission(p.action, p.resource)
    );

    if (hasAllPermissions && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!hasAllPermissions && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}

