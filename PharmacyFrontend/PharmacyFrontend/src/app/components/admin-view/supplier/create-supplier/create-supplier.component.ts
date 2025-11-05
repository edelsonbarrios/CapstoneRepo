import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-supplier',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './create-supplier.component.html',
  styleUrl: './create-supplier.component.scss'
})
export class CreateSupplierComponent {
  @Input() newSupplier = new Supplier();
  @Output() closeForm = new EventEmitter<void>();
  showSuccessAlert: boolean = false;

  // Error properties
  supplierNameError: string = '';
  contactNumberError: string = '';
  contactPersonError: string = '';
  contactPersonNumberError: string = '';
  addressError: string = '';

  constructor(
    private postService : PostService
  ) {}

  // Method to handle contact number input - only allow numbers
  onContactNumberInput(event: any) {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    // Remove any non-numeric characters
    const numericValue = value.replace(/\D/g, '');
    
    // Update the model with numeric value only
    this.newSupplier.contactNumber = numericValue;
    
    // Update the input display value
    input.value = numericValue;
  }

  // Method to handle contact person number input - only allow numbers
  onContactPersonNumberInput(event: any) {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    // Remove any non-numeric characters
    const numericValue = value.replace(/\D/g, '');
    
    // Update the model with numeric value only
    this.newSupplier.contactPersonNumber = numericValue;
    
    // Update the input display value
    input.value = numericValue;
  }

  createNewSupplier() {
    // Clear previous errors
    this.supplierNameError = '';
    this.contactNumberError = '';
    this.contactPersonError = '';
    this.contactPersonNumberError = '';
    this.addressError = '';

    let hasErrors = false;

    // Validate supplier name
    if (!this.newSupplier.supplierName || this.newSupplier.supplierName.trim() === '') {
      this.supplierNameError = 'Supplier name is required';
      hasErrors = true;
    }

    // Validate contact number
    if (!this.newSupplier.contactNumber || this.newSupplier.contactNumber.trim() === '') {
      this.contactNumberError = 'Contact number is required';
      hasErrors = true;
    } else {
      // Check if contact number is exactly 11 digits and starts with 09
      const contactNumber = this.newSupplier.contactNumber.trim();
      if (contactNumber.length !== 11 || !contactNumber.startsWith('09')) {
        this.contactNumberError = 'Invalid contact';
        hasErrors = true;
      }
    }

    // Validate contact person
    if (!this.newSupplier.contactPerson || this.newSupplier.contactPerson.trim() === '') {
      this.contactPersonError = 'Contact person is required';
      hasErrors = true;
    }

    // Validate contact person number
    if (!this.newSupplier.contactPersonNumber || this.newSupplier.contactPersonNumber.trim() === '') {
      this.contactPersonNumberError = 'Contact person number is required';
      hasErrors = true;
    } else {
      // Check if contact person number is exactly 11 digits and starts with 09
      const contactPersonNumber = this.newSupplier.contactPersonNumber.trim();
      if (contactPersonNumber.length !== 11 || !contactPersonNumber.startsWith('09')) {
        this.contactPersonNumberError = 'Invalid contact';
        hasErrors = true;
      }
    }

    // Validate address
    if (!this.newSupplier.address || this.newSupplier.address.trim() === '') {
      this.addressError = 'Address is required';
      hasErrors = true;
    }

    if (hasErrors) {
      return;
    }

    this.postService.createSupplierAsync(this.newSupplier).subscribe({
      next: res => {
        console.log(this.newSupplier);
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        console.log(this.newSupplier);
      }
    });
  }

  closeCreateSupplierForm() {
    // Clear errors when closing form
    this.supplierNameError = '';
    this.contactNumberError = '';
    this.contactPersonError = '';
    this.contactPersonNumberError = '';
    this.addressError = '';
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateSupplierForm();
  }
}