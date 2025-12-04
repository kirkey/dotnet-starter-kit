import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe, PercentPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { Project, ProjectStatus, Customer } from '@core/models/accounting.model';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, PercentPipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, MatProgressBarModule, PageHeaderComponent
  ],
  template: `
    <div class="projects-container">
      <app-page-header 
        title="Projects" 
        subtitle="Manage project accounting and cost tracking"
        icon="work">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search projects</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Planning">Planning</mat-option>
              <mat-option value="Active">Active</mat-option>
              <mat-option value="OnHold">On Hold</mat-option>
              <mat-option value="Completed">Completed</mat-option>
              <mat-option value="Cancelled">Cancelled</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Customer</mat-label>
            <mat-select [value]="selectedCustomer()" (selectionChange)="onCustomerChange($event.value)">
              <mat-option value="">All Customers</mat-option>
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openProjectDialog()">
          <mat-icon>add</mat-icon>
          New Project
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon active">
              <mat-icon>play_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Active Projects</span>
              <span class="summary-value">{{ activeCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon budget">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Budget</span>
              <span class="summary-value">{{ totalBudget() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon spent">
              <mat-icon>trending_up</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Spent</span>
              <span class="summary-value">{{ totalSpent() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon revenue">
              <mat-icon>payments</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Revenue Recognized</span>
              <span class="summary-value">{{ totalRevenue() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="table-container">
          <table mat-table [dataSource]="filteredProjects()" matSort>
            <ng-container matColumnDef="projectNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Project #</th>
              <td mat-cell *matCellDef="let project">
                <span class="project-number">{{ project.projectNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
              <td mat-cell *matCellDef="let project">
                <div class="project-name">{{ project.name }}</div>
                @if (project.customerName) {
                  <div class="project-customer">{{ project.customerName }}</div>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="dates">
              <th mat-header-cell *matHeaderCellDef>Duration</th>
              <td mat-cell *matCellDef="let project">
                <div class="date-range">{{ project.startDate | date:'shortDate' }} - {{ project.endDate | date:'shortDate' }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="budget">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Budget</th>
              <td mat-cell *matCellDef="let project">{{ project.budgetAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="progress">
              <th mat-header-cell *matHeaderCellDef>Progress</th>
              <td mat-cell *matCellDef="let project">
                <div class="progress-cell">
                  <mat-progress-bar mode="determinate" [value]="project.percentComplete"></mat-progress-bar>
                  <span class="progress-text">{{ project.percentComplete }}%</span>
                </div>
              </td>
            </ng-container>

            <ng-container matColumnDef="spent">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Spent</th>
              <td mat-cell *matCellDef="let project" [class.over-budget]="project.actualCost > project.budgetAmount">
                {{ project.actualCost | currency }}
              </td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let project">
                <mat-chip [class]="'status-' + project.status.toLowerCase()">{{ project.status }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let project">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewProject(project)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="openProjectDialog(project)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  @if (project.status === 'Active') {
                    <button mat-menu-item (click)="holdProject(project)">
                      <mat-icon>pause_circle</mat-icon>
                      <span>Put On Hold</span>
                    </button>
                    <button mat-menu-item (click)="completeProject(project)">
                      <mat-icon>check_circle</mat-icon>
                      <span>Mark Complete</span>
                    </button>
                  }
                  @if (project.status === 'OnHold') {
                    <button mat-menu-item (click)="resumeProject(project)">
                      <mat-icon>play_circle</mat-icon>
                      <span>Resume</span>
                    </button>
                  }
                  @if (project.status === 'Planning') {
                    <button mat-menu-item (click)="startProject(project)">
                      <mat-icon>play_arrow</mat-icon>
                      <span>Start Project</span>
                    </button>
                  }
                  <button mat-menu-item (click)="viewTransactions(project)">
                    <mat-icon>receipt_long</mat-icon>
                    <span>Transactions</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator [length]="totalProjects()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #projectDialog>
      <h2 mat-dialog-title>{{ editingProject() ? 'Edit Project' : 'New Project' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="projectForm" class="project-form">
          <div class="form-row">
            <mat-form-field appearance="outline" class="flex-1">
              <mat-label>Project Number</mat-label>
              <input matInput formControlName="projectNumber" required>
            </mat-form-field>

            <mat-form-field appearance="outline" class="flex-2">
              <mat-label>Project Name</mat-label>
              <input matInput formControlName="name" required>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Customer (Optional)</mat-label>
            <mat-select formControlName="customerId">
              <mat-option value="">Internal Project</mat-option>
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3"></textarea>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Start Date</mat-label>
              <input matInput [matDatepicker]="startPicker" formControlName="startDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
              <mat-datepicker #startPicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>End Date</mat-label>
              <input matInput [matDatepicker]="endPicker" formControlName="endDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
              <mat-datepicker #endPicker></mat-datepicker>
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Budget Amount</mat-label>
              <input matInput type="number" formControlName="budgetAmount" required>
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select formControlName="status" required>
                <mat-option value="Planning">Planning</mat-option>
                <mat-option value="Active">Active</mat-option>
                <mat-option value="OnHold">On Hold</mat-option>
                <mat-option value="Completed">Completed</mat-option>
              </mat-select>
            </mat-form-field>
          </div>

          @if (editingProject()) {
            <div class="form-row">
              <mat-form-field appearance="outline">
                <mat-label>Percent Complete</mat-label>
                <input matInput type="number" formControlName="percentComplete" min="0" max="100">
                <span matSuffix>%</span>
              </mat-form-field>
            </div>
          }
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="projectForm.invalid || isSaving()" (click)="saveProject()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .projects-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.active { background: #dbeafe; color: #2563eb; }
    .summary-icon.budget { background: #e0e7ff; color: #4f46e5; }
    .summary-icon.spent { background: #fef3c7; color: #d97706; }
    .summary-icon.revenue { background: #dcfce7; color: #16a34a; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .project-number { font-weight: 500; color: var(--primary-color); }
    .project-name { font-weight: 500; }
    .project-customer { font-size: 12px; color: var(--text-secondary); }
    .date-range { font-size: 13px; }
    .progress-cell { display: flex; align-items: center; gap: 8px; min-width: 120px; }
    .progress-text { font-size: 12px; min-width: 35px; }
    .over-budget { color: #dc2626; font-weight: 500; }
    .status-planning { background: #e0e7ff !important; color: #4338ca !important; }
    .status-active { background: #dbeafe !important; color: #1d4ed8 !important; }
    .status-onhold { background: #fef3c7 !important; color: #92400e !important; }
    .status-completed { background: #dcfce7 !important; color: #166534 !important; }
    .status-cancelled { background: #fee2e2 !important; color: #991b1b !important; }
    .project-form { display: flex; flex-direction: column; gap: 16px; min-width: 500px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .flex-1 { flex: 1; }
    .flex-2 { flex: 2; }
    .full-width { width: 100%; }
  `]
})
export class ProjectsComponent implements OnInit {
  @ViewChild('projectDialog') projectDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  projects = signal<Project[]>([]);
  customers = signal<Customer[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedStatus = signal('');
  selectedCustomer = signal('');
  editingProject = signal<Project | null>(null);

  pageSize = signal(10);
  totalProjects = signal(0);

  displayedColumns = ['projectNumber', 'name', 'dates', 'budget', 'progress', 'spent', 'status', 'actions'];

  projectForm: FormGroup = this.fb.group({
    projectNumber: ['', Validators.required],
    name: ['', Validators.required],
    customerId: [''],
    description: [''],
    startDate: [new Date(), Validators.required],
    endDate: [new Date(), Validators.required],
    budgetAmount: [0, [Validators.required, Validators.min(0)]],
    status: ['Planning', Validators.required],
    percentComplete: [0]
  });

  filteredProjects = computed(() => {
    let result = this.projects();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(p => p.projectCode.toLowerCase().includes(query) || p.projectName.toLowerCase().includes(query));
    if (this.selectedStatus()) result = result.filter(p => p.status === this.selectedStatus());
    if (this.selectedCustomer()) result = result.filter(p => p.clientId === this.selectedCustomer());
    return result;
  });

  activeCount = computed(() => this.projects().filter(p => p.status === ProjectStatus.Active).length);
  totalBudget = computed(() => this.projects().filter(p => p.status === ProjectStatus.Active).reduce((sum, p) => sum + p.budgetedAmount, 0));
  totalSpent = computed(() => this.projects().reduce((sum, p) => sum + p.actualAmount, 0));
  totalRevenue = computed(() => this.projects().reduce((sum, p) => sum + p.billedAmount, 0));

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockProjects: Project[] = [
        { id: '1', projectCode: 'PRJ-001', projectName: 'Website Redesign', clientId: '1', clientName: 'Acme Corp', description: 'Complete website overhaul', startDate: new Date('2024-01-01'), endDate: new Date('2024-06-30'), budgetedAmount: 75000, actualAmount: 32500, billedAmount: 45000, costToComplete: 42500, percentComplete: 55, profitMargin: 15, status: ProjectStatus.Active, isActive: true },
        { id: '2', projectCode: 'PRJ-002', projectName: 'ERP Implementation', clientId: '2', clientName: 'Tech Solutions', description: 'Full ERP system deployment', startDate: new Date('2024-02-15'), endDate: new Date('2024-12-31'), budgetedAmount: 250000, actualAmount: 85000, billedAmount: 100000, costToComplete: 165000, percentComplete: 35, profitMargin: 20, status: ProjectStatus.Active, isActive: true },
        { id: '3', projectCode: 'PRJ-003', projectName: 'Mobile App Development', startDate: new Date('2024-03-01'), endDate: new Date('2024-09-30'), budgetedAmount: 120000, actualAmount: 0, billedAmount: 0, costToComplete: 120000, percentComplete: 0, profitMargin: 25, status: ProjectStatus.Planning, isActive: true }
      ];
      const mockCustomers: Customer[] = [
        { id: '1', customerNumber: 'C001', customerName: 'Acme Corp', creditLimit: 100000, currentBalance: 45000, creditHold: false, isActive: true },
        { id: '2', customerNumber: 'C002', customerName: 'Tech Solutions', creditLimit: 50000, currentBalance: 25000, creditHold: false, isActive: true }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.projects.set(mockProjects);
      this.customers.set(mockCustomers);
      this.totalProjects.set(mockProjects.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }
  onCustomerChange(value: string): void { this.selectedCustomer.set(value); }

  openProjectDialog(project?: Project): void {
    this.editingProject.set(project || null);
    if (project) this.projectForm.patchValue(project);
    else this.projectForm.reset({ startDate: new Date(), endDate: new Date(), budgetAmount: 0, status: 'Planning', percentComplete: 0 });
    this.dialog.open(this.projectDialogTemplate, { width: '600px' });
  }

  async saveProject(): Promise<void> {
    if (this.projectForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Project saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewProject(project: Project): void {}
  viewTransactions(project: Project): void {}
  async startProject(project: Project): Promise<void> { this.notification.success('Project started'); this.loadData(); }
  async holdProject(project: Project): Promise<void> { this.notification.success('Project on hold'); this.loadData(); }
  async resumeProject(project: Project): Promise<void> { this.notification.success('Project resumed'); this.loadData(); }
  async completeProject(project: Project): Promise<void> { this.notification.success('Project completed'); this.loadData(); }
}
