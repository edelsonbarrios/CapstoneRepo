import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { DeleteService } from '../../../../services/DeleteService/delete.service';
import { UpdateService } from '../../../../services/UpdateService/update.service'; // ADD THIS
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, DatePipe } from '@angular/common';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { RestoreConfirmationComponent } from '../../../shared/restore-confirmation/restore-confirmation.component';

// Inline Interfaces
interface BackupFile {
  fileName: string;
  filePath: string;
  fileSize: number;
  createdAt: string;
  backupType: string;
}

interface BackupSettings {
  isEnabled: boolean;
  scheduleType: string;
  scheduleTime: string;
  retentionDays: number;
  backupPath: string;
}

@Component({
  selector: 'app-database-backup',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, DatePipe, ArchiveConfirmationComponent, RestoreConfirmationComponent],
  templateUrl: './database-backup.component.html',
  styleUrl: './database-backup.component.scss',
})
export class DatabaseBackupComponent implements OnInit {
  backups: BackupFile[] = [];
  paginatedBackups: BackupFile[] = [];
  settings: BackupSettings = {
    isEnabled: true,
    scheduleType: 'Daily',
    scheduleTime: '02:00:00', // FIXED: Added default with seconds
    retentionDays: 30,
    backupPath: ''
  };

  isLoading: boolean = false;
  isCreatingBackup: boolean = false;
  errorMessage: string = '';
  showSuccessAlert: boolean = false;
  successMessage: string = '';
  
  showErrorMessage: boolean = false;
  errorAlertMessage: string = '';
  
  // For delete confirmation
  showDeleteForm: boolean = false;
  selectedDeleteFileName: string = '';
  
  // For restore confirmation
  showRestoreForm: boolean = false;
  selectedRestoreFileName: string = '';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private postService: PostService,
    private deleteService: DeleteService,
    private updateService: UpdateService // ADD THIS INJECTION
  ) { }

  ngOnInit(): void {
    this.loadBackups();
    this.loadSettings();
  }

  // Load all backups
  loadBackups() {
    this.isLoading = true;
    this.getService.getBackupListAsync().subscribe({
      next: (response) => {
        this.backups = (response.backups || []).map((b: any) => ({
          fileName: b.fileName,
          filePath: b.filePath || '',
          fileSize: b.fileSizeBytes || 0,
          createdAt: b.createdDate || '',
          backupType: b.backupType || 'Unknown'
        }));
        this.updatePagination();
        this.isLoading = false;
      },
      error: (err) => {
        this.errorAlertMessage = 'Failed to load backups';
        this.showErrorMessage = true;
        this.isLoading = false;
        console.error(err);
      },
    });
  }

  // Load backup settings
  loadSettings() {
    this.getService.getBackupSettingsAsync().subscribe({
      next: (response) => {
        // FIXED: Ensure scheduleTime has a value and convert HH:MM:SS to HH:MM for input
        if (response.scheduleTime) {
          const timeParts = response.scheduleTime.split(':');
          response.scheduleTime = `${timeParts[0]}:${timeParts[1]}`; // Convert to HH:MM for input
        } else {
          response.scheduleTime = '02:00'; // Fallback default
        }
        this.settings = response;
      },
      error: (err) => {
        console.error('Failed to load settings:', err);
        // Set default values if loading fails
        this.settings = {
          isEnabled: true,
          scheduleType: 'Daily',
          scheduleTime: '02:00',
          retentionDays: 30,
          backupPath: ''
        };
      },
    });
  }

  // Update pagination
  updatePagination() {
    this.totalPages = Math.ceil(this.backups.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedBackups = this.backups.slice(startIndex, endIndex);
  }

  // Navigate pages
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  // Create manual backup
  createManualBackup() {
    this.isCreatingBackup = true;
    this.postService.createManualBackupAsync().subscribe({
      next: (response) => {
        this.successMessage = 'Manual backup created successfully!';
        this.showSuccessAlert = true;
        this.isCreatingBackup = false;
      },
      error: (err) => {
        this.errorAlertMessage = err.error?.message || 'Failed to create backup';
        this.showErrorMessage = true;
        this.isCreatingBackup = false;
      },
    });
  }

  // Open delete confirmation
  openDeleteForm(filename: string) {
    this.selectedDeleteFileName = filename;
    this.showDeleteForm = true;
  }

  // Close delete confirmation
  closeDeleteForm() {
    this.showDeleteForm = false;
    this.selectedDeleteFileName = '';
  }

  // Delete backup
  deleteBackup() {
    this.deleteService.deleteBackupAsync(this.selectedDeleteFileName).subscribe({
      next: (response) => {
        this.successMessage = 'Backup deleted successfully!';
        this.showSuccessAlert = true;
        this.showDeleteForm = false;
        this.selectedDeleteFileName = '';
      },
      error: (err) => {
        this.errorAlertMessage = err.error?.message || 'Failed to delete backup';
        this.showErrorMessage = true;
        this.showDeleteForm = false;
        this.selectedDeleteFileName = '';
      }
    });
  }

  // Open restore confirmation
  openRestoreForm(fileName: string) {
    this.selectedRestoreFileName = fileName;
    this.showRestoreForm = true;
  }

  // Close restore confirmation
  closeRestoreForm() {
    this.showRestoreForm = false;
    this.selectedRestoreFileName = '';
  }

  // Restore backup
  restoreBackup() {
    this.postService.restoreBackupAsync(this.selectedRestoreFileName).subscribe({
      next: (response) => {
        this.successMessage = 'Database restored successfully!';
        this.showSuccessAlert = true;
        this.showRestoreForm = false;
        this.selectedRestoreFileName = '';
      },
      error: (err) => {
        this.errorAlertMessage = err.error?.message || 'Failed to restore backup';
        this.showErrorMessage = true;
        this.showRestoreForm = false;
        this.selectedRestoreFileName = '';
      }
    });
  }

  // Download backup
  downloadBackup(filename: string) {
    this.getService.downloadBackupAsync(filename).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        this.errorAlertMessage = 'Failed to download backup';
        this.showErrorMessage = true;
      }
    });
  }

  updateSettings() {
    // Validation before sending
    if (!this.settings.scheduleTime || !this.settings.scheduleTime.match(/^\d{2}:\d{2}$/)) {
      this.errorAlertMessage = 'Invalid time format. Use HH:MM';
      this.showErrorMessage = true;
      return;
    }

    if (this.settings.retentionDays < 1) {
      this.errorAlertMessage = 'Retention days must be at least 1';
      this.showErrorMessage = true;
      return;
    }

    // Convert HH:MM to HH:MM:SS format for backend
    const timeWithSeconds = this.settings.scheduleTime + ':00';
    const settingsToSend = {
      ...this.settings,
      scheduleTime: timeWithSeconds
    };

    this.updateService.updateBackupSettingsAsync(settingsToSend).subscribe({
      next: (response) => {
        this.successMessage = 'Settings updated successfully! Restart application to apply changes.';
        this.showSuccessAlert = true;
        this.loadSettings(); // Reload to get updated values
      },
      error: (err) => {
        this.errorAlertMessage = err.error?.message || 'Failed to update settings';
        this.showErrorMessage = true;
        console.error('Update error:', err);
      }
    });
  }

  // Close success alert
  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.loadBackups();
  }

  // Close error message
  closeValidationMessage() {
    this.showErrorMessage = false;
    this.errorAlertMessage = '';
  }

  // Format file size
  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }

  // FIXED: Get next scheduled backup time - Added null checks
  getNextScheduledBackup(): string {
    if (!this.settings.isEnabled) return 'Disabled';
    
    // FIXED: Check if scheduleTime exists and has valid format
    if (!this.settings.scheduleTime || this.settings.scheduleTime.trim() === '') {
      return 'Not configured';
    }
    
    const timeParts = this.settings.scheduleTime.split(':');
    if (timeParts.length < 2) {
      return 'Invalid time format';
    }
    
    const now = new Date();
    const hours = parseInt(timeParts[0]);
    const minutes = parseInt(timeParts[1]);
    
    if (isNaN(hours) || isNaN(minutes)) {
      return 'Invalid time';
    }
    
    const nextBackup = new Date();
    nextBackup.setHours(hours, minutes, 0, 0);
    
    if (nextBackup <= now) {
      nextBackup.setDate(nextBackup.getDate() + 1);
    }
    
    return nextBackup.toLocaleString();
  }
}