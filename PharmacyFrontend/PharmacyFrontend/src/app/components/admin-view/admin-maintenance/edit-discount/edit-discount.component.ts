// ==================== EDIT DISCOUNT COMPONENT ====================
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
export class EditDiscountComponent implements OnInit{
  @Input() editDiscount: Discount = new Discount();
  @Output() closeForm = new EventEmitter<void>();

  discountNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private updateService: UpdateService) {}

  ngOnInit(): void {
    console.log(JSON.stringify(this.editDiscount));
  }

  updateDiscount() {
    this.discountNameError = '';
    if(this.editDiscount.startDate) {
      this.editDiscount.startDate = new Date(this.editDiscount.startDate).toISOString();
    }
    if(this.editDiscount.endDate) {
      this.editDiscount.endDate = new Date(this.editDiscount.endDate).toISOString();
    }

    // Validate discount name
    if (!this.editDiscount.discountName || this.editDiscount.discountName.trim() === '') {
      this.discountNameError = 'Discount name is required';
      return;
    }

    // Validate discount rate
    if (this.editDiscount.discountAmount === null || this.editDiscount.discountAmount === undefined) {
      this.discountNameError = 'Discount rate is required';
      return;
    }

    if(this.editDiscount.discountType == 'Percentage') {
      if(this.editDiscount.discountAmount <= 0 || this.editDiscount.discountAmount >= 100) {
        this.discountNameError = 'Discount Rate must only be between 0 to 100';
        return;
      }
    }

    if(this.editDiscount.discountType = 'Fixed') {
      if(this.editDiscount.discountAmount < 0) {
        this.discountNameError = 'Discount amount must be greater than or equal to 0';
        return;
      }
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
