import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { Classification } from '../../../../models/classification-models/classification';
import { PostService } from '../../../../services/PostService/post.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-classification',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-classification.component.html',
  styleUrl: './add-classification.component.scss'
})
export class AddClassificationComponent {
  @Input() addClassification: Classification = new Classification();
  @Output() closeForm = new EventEmitter<void>();
  
  classificationNameError: string = '';
  
  constructor(
    private postService : PostService
  ) {}

  createNewClassification() {
    // Clear previous error
    this.classificationNameError = '';
    
    // Validate classification name
    if (!this.addClassification.classificationName || this.addClassification.classificationName.trim() === '') {
      this.classificationNameError = 'Classification name is required';
      return;
    }

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
    // Clear error when closing form
    this.classificationNameError = '';
    this.closeForm.emit();
  }
}