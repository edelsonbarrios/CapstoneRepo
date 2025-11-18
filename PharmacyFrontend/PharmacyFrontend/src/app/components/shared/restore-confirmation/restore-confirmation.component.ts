import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-restore-confirmation',
  standalone: true,
  imports: [NgIf],
  templateUrl: './restore-confirmation.component.html',
  styleUrls: ['./restore-confirmation.component.scss']
})
export class RestoreConfirmationComponent {
  @Input() alertMessage: string = '';
  @Input() message: string = '';

  @Output() onRestore = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  showSuccessAlert: boolean = false;

  onCommit() {
    this.showSuccessAlert = true;
  }

  closeRestoreForm() {
    // Cancel button clicked
    this.onCancel.emit();
  }

  closeSuccessAlert() {
    // OK button clicked on success alert
    this.showSuccessAlert = false;
    this.onRestore.emit(); // This triggers the actual restore in parent
  }
}