import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { PostService } from '../../../../services/PostService/post.service';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-formulation',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-formulation.component.html',
  styleUrl: './add-formulation.component.scss'
})
export class AddFormulationComponent {
  @Input() addFormulation: Formulation = new Formulation();
  @Output() closeForm = new EventEmitter<void>();

  formulationNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(private postService: PostService) {}

  createNewFormulation() {
    // Clear previous error
    this.formulationNameError = '';

    // Validate formulation name
    if (!this.addFormulation.formulationName || this.addFormulation.formulationName.trim() === '') {
      this.formulationNameError = 'Formulation name is required';
      return;
    }

    this.postService.createNewFormulationAsync(this.addFormulation).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        // Handle specific error messages from backend
        if (err.status === 400) {
          // Check if it's a validation error
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.formulationNameError = 'Formulation name already exists';
          } else if (typeof err.error === 'string') {
            this.formulationNameError = err.error;
          } else {
            this.formulationNameError = 'Failed to create formulation';
          }
        } else if (err.status === 409) {
          this.formulationNameError = 'Formulation already exists';
        } else if (err.status === 500) {
          this.formulationNameError = 'Server error occurred';
        } else {
          this.formulationNameError = 'An error occurred while creating the formulation';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeAddFormulationForm();
  }

  closeAddFormulationForm() {
    // Clear error when closing form
    this.formulationNameError = '';
    this.closeForm.emit();
  }
}
