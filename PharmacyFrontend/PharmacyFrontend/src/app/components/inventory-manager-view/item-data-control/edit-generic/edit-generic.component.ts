import { Component,EventEmitter,Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Generic } from '../../../../models/generic-models/generic';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-generic',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-generic.component.html',
  styleUrl: './edit-generic.component.scss'
})
export class EditGenericComponent {
  @Input() editGeneric: Generic = new Generic();
  @Output() closeForm = new EventEmitter<void>();
  
  genericNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService : UpdateService
  ) {}

  updateGeneric() {
    this.genericNameError = '';
    
    if (!this.editGeneric.genericName || this.editGeneric.genericName.trim() === '') {
      this.genericNameError = 'Generic name is required';
      return;
    }

    if (!this.editGeneric) return;
    
    this.updateService.updateGenericAsync(this.editGeneric.genericID, this.editGeneric).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.genericNameError = 'Generic name already exists';
          } else if (typeof err.error === 'string') {
            this.genericNameError = err.error;
          } else {
            this.genericNameError = 'Failed to update generic';
          }
        } else if (err.status === 404) {
          this.genericNameError = 'Generic not found';
        } else {
          this.genericNameError = 'An error occurred while updating the generic';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditGenericForm();
  }

  closeEditGenericForm() {
    // Clear error when closing form
    this.genericNameError = '';
    this.closeForm.emit();
  }
}