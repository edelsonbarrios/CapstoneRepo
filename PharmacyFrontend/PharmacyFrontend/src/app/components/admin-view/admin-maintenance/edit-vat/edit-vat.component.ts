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
  
  showSuccessAlert: boolean = false;

  constructor(
    private updateService: UpdateService
  ) {}

  editVAT() {
    if(!this.editVat) return;
    this.updateService.updateVatAsync(this.editVat.vatid, this.editVat).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${JSON.stringify(err.error)}`);
        this.closeEditVatForm();
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeForm.emit();
  }

  closeEditVatForm() {
    this.closeForm.emit();
  }
}