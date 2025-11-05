import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Generic } from '../../../../models/generic-models/generic';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-generic',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-generic.component.html',
  styleUrl: './add-generic.component.scss'
})
export class AddGenericComponent {
  @Input() addGeneric: Generic = new Generic();
  @Output() closeForm = new EventEmitter<void>();

  genericNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private postService: PostService) {}

  createNewGeneric() {
    // Clear previous error
    this.genericNameError = '';

    // Validate generic name
    if (!this.addGeneric.genericName || this.addGeneric.genericName.trim() === '') {
      this.genericNameError = 'Generic name is required';
      return;
    }

    this.postService.createGenericAsync(this.addGeneric).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        // Handle specific error messages from backend
        if (err.status === 400) {
          // Check if it's a validation error
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.genericNameError = 'Generic name already exists';
          } else if (typeof err.error === 'string') {
            this.genericNameError = err.error;
          } else {
            this.genericNameError = 'Failed to create generic';
          }
        } else if (err.status === 409) {
          this.genericNameError = 'Generic already exists';
        } else if (err.status === 500) {
          this.genericNameError = 'Server error occurred';
        } else {
          this.genericNameError = 'An error occurred while creating the generic';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeAddGenericForm();
  }

  closeAddGenericForm() {
    // Clear error when closing form
    this.genericNameError = '';
    this.closeForm.emit();
  }
}
