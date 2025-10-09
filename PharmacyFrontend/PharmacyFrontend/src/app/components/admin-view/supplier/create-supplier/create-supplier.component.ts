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
  emailError: string = '';
  addressError: string = '';

  constructor(
    private postService : PostService
  ) {}

  createNewSupplier() {
    // Clear previous errors
    this.supplierNameError = '';
    this.contactNumberError = '';
    this.emailError = '';
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
    }

    // Validate email
    if (!this.newSupplier.email || this.newSupplier.email.trim() === '') {
      this.emailError = 'Email is required';
      hasErrors = true;
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
      }
    });
  }

  closeCreateSupplierForm() {
    // Clear errors when closing form
    this.supplierNameError = '';
    this.contactNumberError = '';
    this.emailError = '';
    this.addressError = '';
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateSupplierForm();
  }
}