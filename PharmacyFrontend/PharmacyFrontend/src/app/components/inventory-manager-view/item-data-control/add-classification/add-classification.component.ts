import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { Classification } from '../../../../models/classification-models/classification';
import { PostService } from '../../../../services/PostService/post.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-classification',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-classification.component.html',
  styleUrl: './add-classification.component.scss'
})
export class AddClassificationComponent {
  @Input() addClassification: Classification = new Classification();
  @Output() closeForm = new EventEmitter<void>();

  classificationNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private postService: PostService) {}

  createNewClassification() {
    // Clear previous error
    this.classificationNameError = '';

    // Validate classification name
    if (!this.addClassification.classificationName || this.addClassification.classificationName.trim() === '') {
      this.classificationNameError = 'Classification name is required';
      return;
    }

    this.postService.createNewClassificationAsync(this.addClassification).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        // Handle specific error messages from backend
        if (err.status === 400) {
          // Check if it's a validation error
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.classificationNameError = 'Classification name already exists';
          } else if (typeof err.error === 'string') {
            this.classificationNameError = err.error;
          } else {
            this.classificationNameError = 'Failed to create classification';
          }
        } else if (err.status === 409) {
          this.classificationNameError = 'Classification already exists';
        } else if (err.status === 500) {
          this.classificationNameError = 'Server error occurred';
        } else {
          this.classificationNameError = 'An error occurred while creating the classification';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeAddClassificationForm();
  }

  closeAddClassificationForm() {
    // Clear error when closing form
    this.classificationNameError = '';
    this.closeForm.emit();
  }
}
