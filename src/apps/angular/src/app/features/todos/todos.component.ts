import { Component, inject, signal, OnInit, computed, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { Todo } from '@core/models/todo.model';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatCardModule,
    DragDropModule,
    PageHeaderComponent
  ],
  template: `
    <div class="todos-container">
      <app-page-header 
        title="Todo List" 
        subtitle="Manage your tasks and stay organized"
        icon="checklist">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search todos</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by title...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Priority</mat-label>
            <mat-select [value]="filterPriority()" (selectionChange)="filterPriority.set($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="high">High</mat-option>
              <mat-option value="medium">Medium</mat-option>
              <mat-option value="low">Low</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-icon-button [class.active]="viewMode() === 'kanban'" (click)="viewMode.set('kanban')" matTooltip="Kanban View">
            <mat-icon>view_kanban</mat-icon>
          </button>
          <button mat-icon-button [class.active]="viewMode() === 'list'" (click)="viewMode.set('list')" matTooltip="List View">
            <mat-icon>view_list</mat-icon>
          </button>
          <button mat-raised-button color="primary" (click)="openTodoDialog()">
            <mat-icon>add</mat-icon>
            Add Todo
          </button>
        </div>
      </div>

      <!-- Stats -->
      <div class="stats-row">
        <div class="stat-card">
          <mat-icon>pending</mat-icon>
          <div class="stat-info">
            <span class="stat-value">{{ pendingCount() }}</span>
            <span class="stat-label">Pending</span>
          </div>
        </div>
        <div class="stat-card">
          <mat-icon>autorenew</mat-icon>
          <div class="stat-info">
            <span class="stat-value">{{ inProgressCount() }}</span>
            <span class="stat-label">In Progress</span>
          </div>
        </div>
        <div class="stat-card">
          <mat-icon>check_circle</mat-icon>
          <div class="stat-info">
            <span class="stat-value">{{ completedCount() }}</span>
            <span class="stat-label">Completed</span>
          </div>
        </div>
        <div class="stat-card">
          <mat-icon>priority_high</mat-icon>
          <div class="stat-info">
            <span class="stat-value">{{ overdueCount() }}</span>
            <span class="stat-label">Overdue</span>
          </div>
        </div>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Kanban View -->
        @if (viewMode() === 'kanban') {
          <div class="kanban-board" cdkDropListGroup>
            <!-- Pending Column -->
            <div class="kanban-column">
              <div class="column-header pending">
                <mat-icon>pending</mat-icon>
                <span>Pending</span>
                <span class="count">{{ pendingTodos().length }}</span>
              </div>
              <div class="column-content"
                   cdkDropList
                   [cdkDropListData]="pendingTodos()"
                   (cdkDropListDropped)="onDrop($event, 'pending')">
                @for (todo of pendingTodos(); track todo.id) {
                  <div class="todo-card" cdkDrag [cdkDragData]="todo">
                    <ng-container *ngTemplateOutlet="todoCardTemplate; context: { $implicit: todo }"></ng-container>
                  </div>
                }
                @if (pendingTodos().length === 0) {
                  <div class="empty-column">No pending todos</div>
                }
              </div>
            </div>

            <!-- In Progress Column -->
            <div class="kanban-column">
              <div class="column-header in-progress">
                <mat-icon>autorenew</mat-icon>
                <span>In Progress</span>
                <span class="count">{{ inProgressTodos().length }}</span>
              </div>
              <div class="column-content"
                   cdkDropList
                   [cdkDropListData]="inProgressTodos()"
                   (cdkDropListDropped)="onDrop($event, 'in-progress')">
                @for (todo of inProgressTodos(); track todo.id) {
                  <div class="todo-card" cdkDrag [cdkDragData]="todo">
                    <ng-container *ngTemplateOutlet="todoCardTemplate; context: { $implicit: todo }"></ng-container>
                  </div>
                }
                @if (inProgressTodos().length === 0) {
                  <div class="empty-column">No todos in progress</div>
                }
              </div>
            </div>

            <!-- Completed Column -->
            <div class="kanban-column">
              <div class="column-header completed">
                <mat-icon>check_circle</mat-icon>
                <span>Completed</span>
                <span class="count">{{ completedTodos().length }}</span>
              </div>
              <div class="column-content"
                   cdkDropList
                   [cdkDropListData]="completedTodos()"
                   (cdkDropListDropped)="onDrop($event, 'completed')">
                @for (todo of completedTodos(); track todo.id) {
                  <div class="todo-card completed" cdkDrag [cdkDragData]="todo">
                    <ng-container *ngTemplateOutlet="todoCardTemplate; context: { $implicit: todo }"></ng-container>
                  </div>
                }
                @if (completedTodos().length === 0) {
                  <div class="empty-column">No completed todos</div>
                }
              </div>
            </div>
          </div>
        }

        <!-- List View -->
        @if (viewMode() === 'list') {
          <div class="todo-list">
            @for (todo of filteredTodos(); track todo.id) {
              <mat-card class="todo-list-item" [class.completed]="todo.status === 'completed'">
                <div class="todo-checkbox">
                  <mat-checkbox 
                    [checked]="todo.status === 'completed'"
                    (change)="toggleTodoStatus(todo)"
                    color="primary">
                  </mat-checkbox>
                </div>
                <div class="todo-content">
                  <div class="todo-title">{{ todo.title }}</div>
                  @if (todo.description) {
                    <div class="todo-description">{{ todo.description }}</div>
                  }
                  <div class="todo-meta">
                    <mat-chip class="priority-chip" [class]="'priority-' + todo.priority">
                      {{ todo.priority }}
                    </mat-chip>
                    @if (todo.dueDate) {
                      <span class="due-date" [class.overdue]="isOverdue(todo)">
                        <mat-icon>event</mat-icon>
                        {{ todo.dueDate | date:'mediumDate' }}
                      </span>
                    }
                    @for (tag of todo.tags; track tag) {
                      <mat-chip class="tag-chip">{{ tag }}</mat-chip>
                    }
                  </div>
                </div>
                <div class="todo-actions">
                  <button mat-icon-button (click)="openTodoDialog(todo)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button (click)="deleteTodo(todo)" matTooltip="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </div>
              </mat-card>
            }
          </div>
        }
      }
    </div>

    <!-- Todo Card Template -->
    <ng-template #todoCardTemplate let-todo>
      <div class="card-header">
        <mat-chip class="priority-chip" [class]="'priority-' + todo.priority">
          {{ todo.priority }}
        </mat-chip>
        <button mat-icon-button [matMenuTriggerFor]="cardMenu" class="card-menu-btn">
          <mat-icon>more_horiz</mat-icon>
        </button>
        <mat-menu #cardMenu="matMenu">
          <button mat-menu-item (click)="openTodoDialog(todo)">
            <mat-icon>edit</mat-icon>
            <span>Edit</span>
          </button>
          <button mat-menu-item (click)="deleteTodo(todo)">
            <mat-icon>delete</mat-icon>
            <span>Delete</span>
          </button>
        </mat-menu>
      </div>
      <h4 class="card-title">{{ todo.title }}</h4>
      @if (todo.description) {
        <p class="card-description">{{ todo.description }}</p>
      }
      @if (todo.dueDate) {
        <div class="card-due-date" [class.overdue]="isOverdue(todo)">
          <mat-icon>event</mat-icon>
          {{ todo.dueDate | date:'mediumDate' }}
        </div>
      }
      @if (todo.tags && todo.tags.length > 0) {
        <div class="card-tags">
          @for (tag of todo.tags.slice(0, 2); track tag) {
            <span class="tag">{{ tag }}</span>
          }
          @if (todo.tags.length > 2) {
            <span class="more-tags">+{{ todo.tags.length - 2 }}</span>
          }
        </div>
      }
    </ng-template>

    <!-- Todo Dialog -->
    <ng-template #todoDialog>
      <h2 mat-dialog-title>{{ editingTodo() ? 'Edit Todo' : 'Add New Todo' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="todoForm" class="todo-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Title</mat-label>
            <input matInput formControlName="title" placeholder="What needs to be done?">
            @if (todoForm.get('title')?.hasError('required')) {
              <mat-error>Title is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3" placeholder="Add more details..."></textarea>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Priority</mat-label>
              <mat-select formControlName="priority">
                <mat-option value="low">
                  <mat-icon class="priority-icon low">flag</mat-icon>
                  Low
                </mat-option>
                <mat-option value="medium">
                  <mat-icon class="priority-icon medium">flag</mat-icon>
                  Medium
                </mat-option>
                <mat-option value="high">
                  <mat-icon class="priority-icon high">flag</mat-icon>
                  High
                </mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select formControlName="status">
                <mat-option value="pending">Pending</mat-option>
                <mat-option value="in-progress">In Progress</mat-option>
                <mat-option value="completed">Completed</mat-option>
              </mat-select>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Due Date</mat-label>
            <input matInput [matDatepicker]="picker" formControlName="dueDate">
            <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
            <mat-datepicker #picker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Tags (comma separated)</mat-label>
            <input matInput formControlName="tagsInput" placeholder="work, urgent, personal">
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="todoForm.invalid || isSaving()"
                (click)="saveTodo()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          {{ editingTodo() ? 'Update' : 'Create' }}
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .todos-container {
      padding: 24px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
      gap: 16px;
      flex-wrap: wrap;
    }

    .filters {
      display: flex;
      gap: 12px;
      flex: 1;
    }

    .search-field {
      min-width: 250px;
      flex: 1;
      max-width: 400px;
    }

    .filter-field {
      min-width: 120px;
    }

    .actions {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .actions button.active {
      background: rgba(var(--primary-color-rgb, 89, 74, 226), 0.1);
      color: var(--primary-color);
    }

    /* Stats Row */
    .stats-row {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 16px;
      margin-bottom: 24px;
    }

    .stat-card {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 16px 20px;
      background: var(--surface-color);
      border-radius: 12px;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    }

    .stat-card mat-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
      color: var(--primary-color);
    }

    .stat-info {
      display: flex;
      flex-direction: column;
    }

    .stat-value {
      font-size: 24px;
      font-weight: 600;
      color: var(--text-primary);
    }

    .stat-label {
      font-size: 13px;
      color: var(--text-secondary);
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    /* Kanban Board */
    .kanban-board {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 24px;
      min-height: 400px;
    }

    .kanban-column {
      background: var(--surface-color);
      border-radius: 12px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    }

    .column-header {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 16px;
      font-weight: 500;
      border-bottom: 2px solid;
    }

    .column-header.pending {
      border-color: #f59e0b;
      color: #f59e0b;
    }

    .column-header.in-progress {
      border-color: #3b82f6;
      color: #3b82f6;
    }

    .column-header.completed {
      border-color: #10b981;
      color: #10b981;
    }

    .column-header .count {
      margin-left: auto;
      background: rgba(0, 0, 0, 0.1);
      padding: 2px 8px;
      border-radius: 12px;
      font-size: 12px;
    }

    .column-content {
      padding: 12px;
      min-height: 300px;
    }

    .todo-card {
      background: white;
      border-radius: 8px;
      padding: 12px;
      margin-bottom: 12px;
      cursor: grab;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      transition: box-shadow 0.2s, transform 0.2s;
    }

    .todo-card:hover {
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }

    .todo-card.cdk-drag-preview {
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
    }

    .todo-card.completed {
      opacity: 0.7;
    }

    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 8px;
    }

    .card-menu-btn {
      width: 28px;
      height: 28px;
      line-height: 28px;
    }

    .card-menu-btn mat-icon {
      font-size: 18px;
    }

    .card-title {
      font-size: 14px;
      font-weight: 500;
      margin: 0 0 4px;
      color: var(--text-primary);
    }

    .card-description {
      font-size: 12px;
      color: var(--text-secondary);
      margin: 0 0 8px;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    .card-due-date {
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 12px;
      color: var(--text-secondary);
      margin-bottom: 8px;
    }

    .card-due-date mat-icon {
      font-size: 14px;
      width: 14px;
      height: 14px;
    }

    .card-due-date.overdue {
      color: #ef4444;
    }

    .card-tags {
      display: flex;
      gap: 4px;
      flex-wrap: wrap;
    }

    .tag {
      font-size: 10px;
      padding: 2px 8px;
      background: rgba(var(--primary-color-rgb, 89, 74, 226), 0.1);
      color: var(--primary-color);
      border-radius: 12px;
    }

    .more-tags {
      font-size: 10px;
      color: var(--text-secondary);
    }

    .empty-column {
      text-align: center;
      padding: 24px;
      color: var(--text-secondary);
      font-size: 13px;
    }

    /* Priority Chips */
    .priority-chip {
      font-size: 10px;
      min-height: 20px;
      padding: 2px 8px;
    }

    .priority-low {
      background: #dcfce7 !important;
      color: #166534 !important;
    }

    .priority-medium {
      background: #fef3c7 !important;
      color: #92400e !important;
    }

    .priority-high {
      background: #fee2e2 !important;
      color: #991b1b !important;
    }

    /* List View */
    .todo-list {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .todo-list-item {
      display: flex;
      align-items: flex-start;
      gap: 16px;
      padding: 16px;
    }

    .todo-list-item.completed {
      opacity: 0.6;
    }

    .todo-list-item.completed .todo-title {
      text-decoration: line-through;
    }

    .todo-content {
      flex: 1;
    }

    .todo-title {
      font-weight: 500;
      margin-bottom: 4px;
    }

    .todo-description {
      font-size: 13px;
      color: var(--text-secondary);
      margin-bottom: 8px;
    }

    .todo-meta {
      display: flex;
      align-items: center;
      gap: 8px;
      flex-wrap: wrap;
    }

    .due-date {
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 12px;
      color: var(--text-secondary);
    }

    .due-date mat-icon {
      font-size: 14px;
      width: 14px;
      height: 14px;
    }

    .due-date.overdue {
      color: #ef4444;
    }

    .tag-chip {
      font-size: 10px;
      min-height: 20px;
    }

    /* Dialog Form */
    .todo-form {
      display: flex;
      flex-direction: column;
      gap: 8px;
      min-width: 400px;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .priority-icon.low {
      color: #10b981;
    }

    .priority-icon.medium {
      color: #f59e0b;
    }

    .priority-icon.high {
      color: #ef4444;
    }

    @media (max-width: 900px) {
      .kanban-board {
        grid-template-columns: 1fr;
      }
    }

    @media (max-width: 600px) {
      .filters {
        flex-direction: column;
      }

      .search-field {
        max-width: none;
      }

      .todo-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class TodosComponent implements OnInit {
  @ViewChild('todoDialog') todoDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  // State signals
  todos = signal<Todo[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  filterPriority = signal('');
  viewMode = signal<'kanban' | 'list'>('kanban');
  editingTodo = signal<Todo | null>(null);

  todoForm: FormGroup = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    priority: ['medium'],
    status: ['pending'],
    dueDate: [null],
    tagsInput: ['']
  });

  // Computed signals
  filteredTodos = computed(() => {
    let result = this.todos();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(t => t.title.toLowerCase().includes(query));
    }
    
    const priority = this.filterPriority();
    if (priority) {
      result = result.filter(t => t.priority === priority);
    }
    
    return result;
  });

  pendingTodos = computed(() => this.filteredTodos().filter(t => t.status === 'pending'));
  inProgressTodos = computed(() => this.filteredTodos().filter(t => t.status === 'in-progress'));
  completedTodos = computed(() => this.filteredTodos().filter(t => t.status === 'completed'));

  pendingCount = computed(() => this.todos().filter(t => t.status === 'pending').length);
  inProgressCount = computed(() => this.todos().filter(t => t.status === 'in-progress').length);
  completedCount = computed(() => this.todos().filter(t => t.status === 'completed').length);
  overdueCount = computed(() => this.todos().filter(t => this.isOverdue(t)).length);

  ngOnInit(): void {
    this.loadTodos();
  }

  async loadTodos(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      // Mock data - replace with actual API call
      const mockTodos: Todo[] = [
        { id: '1', title: 'Complete project documentation', description: 'Write comprehensive docs for the new API', priority: 'high', status: 'in-progress', dueDate: '2025-01-20', tags: ['work', 'documentation'], createdAt: '2025-01-15' },
        { id: '2', title: 'Review pull requests', priority: 'medium', status: 'pending', dueDate: '2025-01-18', tags: ['work'], createdAt: '2025-01-16' },
        { id: '3', title: 'Setup CI/CD pipeline', description: 'Configure GitHub Actions for automated testing', priority: 'high', status: 'pending', tags: ['devops', 'work'], createdAt: '2025-01-14' },
        { id: '4', title: 'Team meeting prep', priority: 'low', status: 'completed', completedAt: '2025-01-17', createdAt: '2025-01-15' },
        { id: '5', title: 'Update dependencies', description: 'Update npm packages to latest versions', priority: 'medium', status: 'pending', dueDate: '2025-01-25', tags: ['maintenance'], createdAt: '2025-01-16' },
        { id: '6', title: 'Code review session', priority: 'medium', status: 'completed', completedAt: '2025-01-16', createdAt: '2025-01-14' },
      ];
      
      await new Promise(resolve => setTimeout(resolve, 500));
      this.todos.set(mockTodos);
    } catch (error) {
      this.notification.error('Failed to load todos');
    } finally {
      this.isLoading.set(false);
    }
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  isOverdue(todo: Todo): boolean {
    if (!todo.dueDate || todo.status === 'completed') return false;
    return new Date(todo.dueDate) < new Date();
  }

  onDrop(event: CdkDragDrop<Todo[]>, status: 'pending' | 'in-progress' | 'completed'): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const todo = event.previousContainer.data[event.previousIndex];
      this.updateTodoStatus(todo, status);
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  updateTodoStatus(todo: Todo, status: 'pending' | 'in-progress' | 'completed'): void {
    this.todos.update(todos => 
      todos.map(t => t.id === todo.id ? { ...t, status, completedAt: status === 'completed' ? new Date().toISOString() : undefined } : t)
    );
    this.notification.success(`Todo moved to ${status.replace('-', ' ')}`);
  }

  toggleTodoStatus(todo: Todo): void {
    const newStatus = todo.status === 'completed' ? 'pending' : 'completed';
    this.updateTodoStatus(todo, newStatus);
  }

  openTodoDialog(todo?: Todo): void {
    this.editingTodo.set(todo || null);
    
    if (todo) {
      this.todoForm.patchValue({
        title: todo.title,
        description: todo.description || '',
        priority: todo.priority,
        status: todo.status,
        dueDate: todo.dueDate ? new Date(todo.dueDate) : null,
        tagsInput: todo.tags?.join(', ') || ''
      });
    } else {
      this.todoForm.reset({ priority: 'medium', status: 'pending' });
    }

    this.dialog.open(this.todoDialogTemplate, {
      width: '500px'
    });
  }

  async saveTodo(): Promise<void> {
    if (this.todoForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      
      const formValue = this.todoForm.value;
      const todoData: Partial<Todo> = {
        title: formValue.title,
        description: formValue.description,
        priority: formValue.priority,
        status: formValue.status,
        dueDate: formValue.dueDate ? formValue.dueDate.toISOString() : undefined,
        tags: formValue.tagsInput ? formValue.tagsInput.split(',').map((t: string) => t.trim()).filter((t: string) => t) : []
      };

      if (this.editingTodo()) {
        this.todos.update(todos => 
          todos.map(t => t.id === this.editingTodo()!.id ? { ...t, ...todoData } : t)
        );
        this.notification.success('Todo updated successfully');
      } else {
        const newTodo: Todo = {
          ...todoData as Todo,
          id: Date.now().toString(),
          createdAt: new Date().toISOString()
        };
        this.todos.update(todos => [newTodo, ...todos]);
        this.notification.success('Todo created successfully');
      }
      
      this.dialog.closeAll();
    } catch (error) {
      this.notification.error('Failed to save todo');
    } finally {
      this.isSaving.set(false);
    }
  }

  async deleteTodo(todo: Todo): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Todo',
        message: `Are you sure you want to delete "${todo.title}"?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.todos.update(todos => todos.filter(t => t.id !== todo.id));
        this.notification.success('Todo deleted successfully');
      }
    });
  }
}
