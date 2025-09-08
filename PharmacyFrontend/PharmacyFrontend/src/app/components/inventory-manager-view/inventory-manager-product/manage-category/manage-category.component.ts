import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { Product } from '../../../../models/product-models/product';
import { CreateCategoryComponent } from '../create-category/create-category.component';
import { ViewCategoryComponent } from '../view-category/view-category.component';

@Component({
  selector: 'app-manage-category',
  standalone: true,
  imports: [NgIf,CreateCategoryComponent,ViewCategoryComponent],
  templateUrl: './manage-category.component.html',
  styleUrl: './manage-category.component.scss'
})
export class ManageCategoryComponent {
  newCategory ?: Product | null;
  viewCategory ?: Product | null;

  openCreateCategoryForm() {
    this.newCategory= new Product;
  }

  openViewCategoryForm() {
    this.viewCategory= new Product;
  }
}
