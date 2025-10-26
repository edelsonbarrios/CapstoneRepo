import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-formulation',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-formulation.component.html',
  styleUrl: './edit-formulation.component.scss'
})
export class EditFormulationComponent {
  @Input() editFormulation: Formulation = new Formulation();
  @Output() closeForm = new EventEmitter<void>();
  
  formulationNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService : UpdateService
  ) {}

  updateFormulation() {
    this.formulationNameError = '';
    
    if (!this.editFormulation.formulationName || this.editFormulation.formulationName.trim() === '') {
      this.formulationNameError = 'Formulation name is required';
      return;
    }

    this.updateService.updateFormulationAsync(this.editFormulation.formulationID, this.editFormulation).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.formulationNameError = 'Formulation name already exists';
          } else if (typeof err.error === 'string') {
            this.formulationNameError = err.error;
          } else {
            this.formulationNameError = 'Failed to update formulation';
          }
        } else if (err.status === 404) {
          this.formulationNameError = 'Formulation not found';
        } else {
          this.formulationNameError = 'An error occurred while updating the formulation';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditFormulationForm();
  }

  closeEditFormulationForm() {
    // Clear error when closing form
    this.formulationNameError = '';
    this.closeForm.emit();
  }
}