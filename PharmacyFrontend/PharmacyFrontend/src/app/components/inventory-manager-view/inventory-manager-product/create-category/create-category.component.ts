import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { CreateCategory } from '../../../../models/product-models/createCategory';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-category',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-category.component.html',
  styleUrl: './create-category.component.scss'
})
export class CreateCategoryComponent {
  @Input() newCategory: CreateCategory = new CreateCategory;
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService : PostService
  ) {}

  createCategory() {
    if(!this.newCategory) return;
    this.postService.createNewCategoryAsync(this.newCategory).subscribe({
      next: res => {
        alert(`Category successfully created.`);
        this.closeCreateCategoryForm();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateCategoryForm() {
    this.closeForm.emit();
  }
}
