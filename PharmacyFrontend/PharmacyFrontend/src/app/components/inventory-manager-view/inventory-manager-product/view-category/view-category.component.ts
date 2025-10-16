import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Category } from '../../../../models/product-models/category';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-view-category',
  standalone: true,
  imports: [NgIf, FormsModule],
  templateUrl: './view-category.component.html',
  styleUrl: './view-category.component.scss'
})
export class ViewCategoryComponent {
  @Input() viewCategory: Category = new Category; 
  showSuccessAlert : boolean = false;
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private updateService : UpdateService
  ) {}

  updateCategory() {
    this.updateService.updateCategoryAsync(this.viewCategory.categoryID, this.viewCategory).subscribe({
      next: res => {
        console.log(res.message);
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${JSON.stringify(err.error)}`);
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeUpdateCategoryForm();
  }

  closeUpdateCategoryForm() {
    this.closeForm.emit();
  }
}
