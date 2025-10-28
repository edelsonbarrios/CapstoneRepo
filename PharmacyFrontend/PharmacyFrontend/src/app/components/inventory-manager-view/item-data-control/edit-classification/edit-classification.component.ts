import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Classification } from '../../../../models/classification-models/classification';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-classification',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-classification.component.html',
  styleUrl: './edit-classification.component.scss'
})
export class EditClassificationComponent {
  @Input() editClassification: Classification = new Classification();
  @Output() closeForm = new EventEmitter<void>();
  
  classificationNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService : UpdateService
  ) {}

  updateClassification() {
    this.classificationNameError = '';
    
    if (!this.editClassification.classificationName || this.editClassification.classificationName.trim() === '') {
      this.classificationNameError = 'Classification name is required';
      return;
    }

    this.updateService.updateClassificationAsync(this.editClassification.classificationID, this.editClassification).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.classificationNameError = 'Classification name already exists';
          } else if (typeof err.error === 'string') {
            this.classificationNameError = err.error;
          } else {
            this.classificationNameError = 'Failed to update classification';
          }
        } else if (err.status === 404) {
          this.classificationNameError = 'Classification not found';
        } else {
          this.classificationNameError = 'An error occurred while updating the classification';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditClassificationForm();
  }

  closeEditClassificationForm() {
    // Clear error when closing form
    this.classificationNameError = '';
    this.closeForm.emit();
  }
}