import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Brand } from '../../../../models/brand-models/brand';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-brand.component.html',
  styleUrl: './add-brand.component.scss'
})
export class AddBrandComponent {
  @Input() addBrand: Brand = new Brand();
  @Output() closeForm = new EventEmitter<void>();

  brandNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private postService: PostService) {}

  createNewBrand() {
    // Clear previous error
    this.brandNameError = '';

    // Validate brand name
    if (!this.addBrand.brandName || this.addBrand.brandName.trim() === '') {
      this.brandNameError = 'Brand name is required';
      return;
    }

    this.postService.createBrandAsync(this.addBrand).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        // Handle specific error messages from backend
        if (err.status === 400) {
          // Check if it's a validation error
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.brandNameError = 'Brand name already exists';
          } else if (typeof err.error === 'string') {
            this.brandNameError = err.error;
          } else {
            this.brandNameError = 'Failed to create brand';
          }
        } else if (err.status === 409) {
          this.brandNameError = 'Brand already exists';
        } else if (err.status === 500) {
          this.brandNameError = 'Server error occurred';
        } else {
          this.brandNameError = 'An error occurred while creating the brand';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateBrandForm();
  }

  closeCreateBrandForm() {
    // Clear error when closing form
    this.brandNameError = '';
    this.closeForm.emit();
  }
}
