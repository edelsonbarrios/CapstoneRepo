// ==================== EDIT DISCOUNT COMPONENT ====================
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Discount } from '../../../../models/discount-models/discount';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-discount',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-discount.component.html',
  styleUrl: './edit-discount.component.scss'
})
export class EditDiscountComponent {
  @Input() editDiscount: Discount = new Discount();
  @Output() closeForm = new EventEmitter<void>();

  discountNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private updateService: UpdateService) {}

  updateDiscount() {
    this.discountNameError = '';

    // Validate discount name
    if (!this.editDiscount.discountName || this.editDiscount.discountName.trim() === '') {
      this.discountNameError = 'Discount name is required';
      return;
    }

    // Validate discount rate
    if (this.editDiscount.discountRate === null || this.editDiscount.discountRate === undefined) {
      this.discountNameError = 'Discount rate is required';
      return;
    }

    if (this.editDiscount.discountRate < 0 || this.editDiscount.discountRate > 100) {
      this.discountNameError = 'Discount rate must be between 0 and 100';
      return;
    }

    if (!this.editDiscount) return;

    this.updateService.updateDiscountAsync(this.editDiscount.id, this.editDiscount).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.discountNameError = 'Discount name already exists';
          } else if (typeof err.error === 'string') {
            this.discountNameError = err.error;
          } else {
            this.discountNameError = 'Failed to update discount';
          }
        } else if (err.status === 404) {
          this.discountNameError = 'Discount not found';
        } else {
          this.discountNameError = 'An error occurred while updating the discount';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditDiscountForm();
  }

  closeEditDiscountForm() {
    this.discountNameError = '';
    this.closeForm.emit();
  }
}
