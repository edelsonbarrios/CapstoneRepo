import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { Product } from '../../../../models/product-models/product';
import { CreateCategoryComponent } from '../create-category/create-category.component';
import { ViewCategoryComponent } from '../view-category/view-category.component';
import { CreateCategory } from '../../../../models/product-models/createCategory';
import { Category } from '../../../../models/product-models/category';

@Component({
  selector: 'app-manage-category',
  standalone: true,
  imports: [NgIf,CreateCategoryComponent,ViewCategoryComponent],
  templateUrl: './manage-category.component.html',
  styleUrl: './manage-category.component.scss'
})
export class ManageCategoryComponent {
  newCategory ?: CreateCategory | null;
  viewCategory ?: Category | null;

  openCreateCategoryForm() {
    this.newCategory= new CreateCategory;
  }

  openViewCategoryForm() {
    this.viewCategory= new Category;
  }
}
