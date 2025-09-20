import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { Classification } from '../../../../models/classification-models/classification';
import { PostService } from '../../../../services/PostService/post.service';

@Component({
  selector: 'app-add-classification',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-classification.component.html',
  styleUrl: './add-classification.component.scss'
})
export class AddClassificationComponent {
  @Input() addClassification: Classification = new Classification();
  @Output() closeForm = new EventEmitter<void>();
  constructor(
    private postService : PostService
  ) {}

  createNewClassification() {
    this.postService.createNewClassificationAsync(this.addClassification).subscribe({
      next: res => {
        alert(`Classification successfully created.`);
        this.closeAddClassificationForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.addClassification.classificationName = '';
      }
    });
  }

  closeAddClassificationForm() {
    this.closeForm.emit();
  }
}
