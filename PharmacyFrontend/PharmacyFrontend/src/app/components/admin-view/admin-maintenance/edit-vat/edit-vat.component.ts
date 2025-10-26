import { Component, EventEmitter, Input, Output } from '@angular/core';
import { VAT } from '../../../../models/vat-models/vat';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-vat',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-vat.component.html',
  styleUrl: './edit-vat.component.scss'
})
export class EditVatComponent {
  @Input() editVat: VAT = new VAT();
  @Output() closeForm = new EventEmitter<void>();
  
  vatNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService: UpdateService
  ) {}

 editVAT() {
    this.vatNameError = '';
    
    if (!this.editVat.vatName || this.editVat.vatName.trim() === '') {
      this.vatNameError = 'VAT name is required';
      return;
    }

    if (!this.editVat) return;
    
    this.updateService.updateVatAsync(this.editVat.vatid, this.editVat).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${JSON.stringify(err.error)}`);
        
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.vatNameError = 'VAT name already exists';
          } else if (typeof err.error === 'string') {
            this.vatNameError = err.error;
          } else {
            this.vatNameError = 'Failed to update VAT';
          }
        } else if (err.status === 404) {
          this.vatNameError = 'VAT not found';
        } else {
          this.vatNameError = 'An error occurred while updating the VAT';
        }
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditVatForm();
  }

  closeEditVatForm() {
    this.vatNameError = '';
    this.closeForm.emit();
  }
}