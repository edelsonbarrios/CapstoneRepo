import { Component,EventEmitter,Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Generic } from '../../../../models/generic-models/generic';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-generic',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-generic.component.html',
  styleUrl: './edit-generic.component.scss'
})
export class EditGenericComponent {
  @Input() editGeneric: Generic = new Generic();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService : UpdateService
  ) {}

  updateGeneric() {
    if(!this.editGeneric) return;
    this.updateService.updateGenericAsync(this.editGeneric.genericID, this.editGeneric).subscribe({
      next: res => {
        alert(`Generic successfully updated.`);
        this.closeEditGenericForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.editGeneric.genericName = '';
      }
    });
  }

  closeEditGenericForm() {
    this.closeForm.emit();
  }
}
