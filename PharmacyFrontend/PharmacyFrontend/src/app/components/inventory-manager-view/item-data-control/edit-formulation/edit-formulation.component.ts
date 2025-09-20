import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-formulation',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-formulation.component.html',
  styleUrl: './edit-formulation.component.scss'
})
export class EditFormulationComponent {
  @Input() editFormulation: Formulation = new Formulation();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService : UpdateService
  ) {}

  updateFormulation() {
    this.updateService.updateFormulationAsync(this.editFormulation.formulationID, this.editFormulation).subscribe({
      next: res => {
        alert(`Formulation sucessfully updated.`);
        this.closeEditFormulationForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.editFormulation.formulationName = '';
      }
    });
  }

  closeEditFormulationForm() {
    this.closeForm.emit();
  }
}
