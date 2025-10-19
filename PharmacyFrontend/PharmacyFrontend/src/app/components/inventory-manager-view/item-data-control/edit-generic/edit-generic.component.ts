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
    // Clear previous error
    this.genericNameError = '';
    
    // Validate generic name
    if (!this.editGeneric.genericName || this.editGeneric.genericName.trim() === '') {
      this.genericNameError = 'Generic name is required';
      return;
    }

    if(!this.editGeneric) return;
    this.updateService.updateGenericAsync(this.editGeneric.genericID, this.editGeneric).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        this.editGeneric.genericName = '';
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