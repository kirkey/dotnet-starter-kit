import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { JournalEntry } from '@core/models/accounting.model';

@Component({
  selector: 'app-approve-reject-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>
      <mat-icon [class]="action === 'approve' ? 'approve-icon' : 'reject-icon'">
        {{ action === 'approve' ? 'check_circle' : 'cancel' }}
      </mat-icon>
      {{ action === 'approve' ? 'Approve' : 'Reject' }} Journal Entry
    </h2>

    <mat-dialog-content>
      <div class="entry-info">
        <div class="info-item">
          <span class="label">Entry Number:</span>
          <span class="value">{{ entry.entryNumber }}</span>
        </div>
        <div class="info-item">
          <span class="label">Date:</span>
          <span class="value">{{ entry.entryDate | date:'shortDate' }}</span>
        </div>
        <div class="info-item">
          <span class="label">Description:</span>
          <span class="value">{{ entry.description }}</span>
        </div>
        <div class="info-item">
          <span class="label">Total Amount:</span>
          <span class="value">{{ entry.totalDebits | currency }}</span>
        </div>
      </div>

      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>{{ action === 'approve' ? 'Approval Notes' : 'Rejection Reason' }}</mat-label>
          <textarea matInput formControlName="comments" rows="4"
                    [placeholder]="action === 'approve' ? 'Optional notes for approval' : 'Please provide a reason for rejection'"></textarea>
          @if (action === 'reject' && form.get('comments')?.hasError('required')) {
            <mat-error>Rejection reason is required</mat-error>
          }
        </mat-form-field>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button
              [color]="action === 'approve' ? 'primary' : 'warn'"
              [disabled]="form.invalid || processing()"
              (click)="submit()">
        @if (processing()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ action === 'approve' ? 'Approve' : 'Reject' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 400px;
    }

    h2 {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .approve-icon {
      color: #388e3c;
    }

    .reject-icon {
      color: #c62828;
    }

    .entry-info {
      background-color: #f5f5f5;
      padding: 16px;
      border-radius: 8px;
      margin-bottom: 24px;
    }

    .info-item {
      display: flex;
      justify-content: space-between;
      margin-bottom: 8px;
    }

    .info-item:last-child {
      margin-bottom: 0;
    }

    .label {
      color: #666;
    }

    .value {
      font-weight: 500;
    }

    .full-width {
      width: 100%;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class ApproveRejectDialogComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ApproveRejectDialogComponent>);
  private data = inject<{ entry: JournalEntry; action: 'approve' | 'reject' }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form: FormGroup;
  processing = signal(false);

  get entry(): JournalEntry {
    return this.data.entry;
  }

  get action(): 'approve' | 'reject' {
    return this.data.action;
  }

  constructor() {
    const isReject = this.data.action === 'reject';
    this.form = this.fb.group({
      comments: ['', isReject ? Validators.required : []]
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.processing.set(true);
    const comments = this.form.value.comments;

    if (this.action === 'approve') {
      this.accountingService.approveJournalEntry({
        id: this.entry.id,
        comments: comments
      }).subscribe({
        next: () => {
          this.snackBar.open('Journal entry approved', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error approving entry:', error);
          this.snackBar.open('Failed to approve entry', 'Close', { duration: 3000 });
          this.processing.set(false);
        }
      });
    } else {
      this.accountingService.rejectJournalEntry({
        id: this.entry.id,
        reason: comments
      }).subscribe({
        next: () => {
          this.snackBar.open('Journal entry rejected', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error rejecting entry:', error);
          this.snackBar.open('Failed to reject entry', 'Close', { duration: 3000 });
          this.processing.set(false);
        }
      });
    }
  }
}
