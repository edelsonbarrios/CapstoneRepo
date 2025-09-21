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

  constructor(
    private updateService : UpdateService
  ) {}

  updateFormulation() {
    // Clear previous error
    this.formulationNameError = '';
    
    // Validate formulation name
    if (!this.editFormulation.formulationName || this.editFormulation.formulationName.trim() === '') {
      this.formulationNameError = 'Formulation name is required';
      return;
    }

    this.updateService.updateFormulationAsync(this.editFormulation.formulationID, this.editFormulation).subscribe({
      next: res => {
        alert(`Formulation sucessfully updated.`);
        this.closeEditFormulationForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.editFormulation.formulationName = '';
      }
    });
  }

  closeEditFormulationForm() {
    // Clear error when closing form
    this.formulationNameError = '';
    this.closeForm.emit();
  }
}