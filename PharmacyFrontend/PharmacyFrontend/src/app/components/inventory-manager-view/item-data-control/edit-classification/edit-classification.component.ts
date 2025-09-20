import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Classification } from '../../../../models/classification-models/classification';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-classification',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-classification.component.html',
  styleUrl: './edit-classification.component.scss'
})
export class EditClassificationComponent {
  @Input() editClassification: Classification = new Classification();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService : UpdateService
  ) {}

  updateClassification() {
    this.updateService.updateClassificationAsync(this.editClassification.classificationID, this.editClassification).subscribe({
      next: res => {
        alert(`Classification successfully updated.`);
        this.closeEditClassificationForm();
      },
      error : err => {
        console.log(`${err.error}`);
        this.editClassification.classificationName = '';
      }
    });
  }

  closeEditClassificationForm() {
    this.closeForm.emit();
  }
}
