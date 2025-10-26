import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { TypeModel } from '../../../../models/type-models/type';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { endWith } from 'rxjs';

@Component({
  selector: 'app-edit-type',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-type.component.html',
  styleUrl: './edit-type.component.scss'
})
export class EditTypeComponent {
  @Input() editType: TypeModel = new TypeModel();
  @Output() closeForm = new EventEmitter<void>();
  
  typeNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService : UpdateService
  ) {}

  updateType() {
    this.typeNameError = '';
    
    if (!this.editType.typeName || this.editType.typeName.trim() === '') {
      this.typeNameError = 'Type name is required';
      return;
    }

    if (!this.editType) return;
    
    this.updateService.updateTypeAsync(this.editType.typeID, this.editType).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.typeNameError = 'Type name already exists';
          } else if (typeof err.error === 'string') {
            this.typeNameError = err.error;
          } else {
            this.typeNameError = 'Failed to update type';
          }
        } else if (err.status === 404) {
          this.typeNameError = 'Type not found';
        } else {
          this.typeNameError = 'An error occurred while updating the type';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditTypeForm();
  }

  closeEditTypeForm() {
    // Clear error when closing form
    this.typeNameError = '';
    this.closeForm.emit();
  }
}