import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { VAT } from '../../../../models/vat-models/vat';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-vat',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-vat.component.html',
  styleUrl: './edit-vat.component.scss'
})
export class EditVatComponent {
  @Input() editVat: VAT = new VAT();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService: UpdateService
  ) {}

  editVAT() {
    if(!this.editVAT) return;
    this.updateService.updateVatAsync(this.editVat.vatid, this.editVat).subscribe({
      next: res => {
        alert(`${res}`);
        this.closeForm.emit();
      },
      error: err => {
        console.log(`${JSON.stringify(err.error)}`);
        this.closeEditVatForm();
      }
    });
  }

  closeEditVatForm() {
    this.closeForm.emit();
  }
}
