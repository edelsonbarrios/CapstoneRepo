import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { PostService } from '../../../../services/PostService/post.service';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-formulation',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-formulation.component.html',
  styleUrl: './add-formulation.component.scss'
})
export class AddFormulationComponent {
  @Input() addFormulation: Formulation = new Formulation();
  @Output() closeForm = new EventEmitter<void>();
  constructor(
    private postService : PostService
  ) {}

  createNewFormulation() {
    this.postService.createNewFormulationAsync(this.addFormulation).subscribe({
      next: res => {
        alert(`Formulation successfully created.`);
        this.closeAddFormulationForm();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeAddFormulationForm() {
    this.closeForm.emit();
  }
}
