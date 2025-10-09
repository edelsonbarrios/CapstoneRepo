import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { CreateCategory } from '../../../../models/product-models/createCategory';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-category',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './create-category.component.html',
  styleUrl: './create-category.component.scss'
})
export class CreateCategoryComponent {
  @Input() newCategory: CreateCategory = new CreateCategory;
  @Output() closeForm = new EventEmitter<void>();
  showSuccessAlert: boolean = false;

  constructor(
    private postService : PostService
  ) {}

  createCategory() {
    if(!this.newCategory) return;
    this.postService.createNewCategoryAsync(this.newCategory).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateCategoryForm() {
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateCategoryForm();
  }
}