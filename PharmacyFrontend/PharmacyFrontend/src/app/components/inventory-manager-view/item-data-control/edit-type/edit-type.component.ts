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

  constructor(
    private updateService : UpdateService
  ) {}

  updateType() {
    // Clear previous error
    this.typeNameError = '';
    
    // Validate type name
    if (!this.editType.typeName || this.editType.typeName.trim() === '') {
      this.typeNameError = 'Type name is required';
      return;
    }

    if(!this.editType) return;
    this.updateService.updateTypeAsync(this.editType.typeID, this.editType).subscribe({
      next: res => {
        alert(`Type successfully updated.`);
        this.closeEditTypeForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.editType.typeName = '';
      }
    });
  }

  closeEditTypeForm() {
    // Clear error when closing form
    this.typeNameError = '';
    this.closeForm.emit();
  }
}