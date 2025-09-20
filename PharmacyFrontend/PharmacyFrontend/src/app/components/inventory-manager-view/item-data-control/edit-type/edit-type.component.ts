import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { TypeModel } from '../../../../models/type-models/type';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';
import { endWith } from 'rxjs';

@Component({
  selector: 'app-edit-type',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-type.component.html',
  styleUrl: './edit-type.component.scss'
})
export class EditTypeComponent {
  @Input() editType: TypeModel = new TypeModel();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService : UpdateService
  ) {}

  updateType() {
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
    this.closeForm.emit();
  }
}
