import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-delete-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-confirmation.component.html',
  styleUrl: './delete-confirmation.component.scss'
})
export class DeleteConfirmationComponent {
  @Input() message: string = 'Are you sure you want to delete this item?';
  @Input() alertMessage: string = 'Item has been deleted successfully!';
  @Output() onDelete = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  showConfirmation: boolean = true;
  showSuccessAlert: boolean = false;

  confirmDelete() {
    this.showConfirmation = false;
    this.showSuccessAlert = true;
  }

  cancelDelete() {
    this.onCancel.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.onDelete.emit();
  }
}