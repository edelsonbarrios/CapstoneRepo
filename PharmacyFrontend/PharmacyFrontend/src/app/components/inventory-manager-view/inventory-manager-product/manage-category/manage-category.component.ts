import { Component, OnInit } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { Product } from '../../../../models/product-models/product';
import { CreateCategoryComponent } from '../create-category/create-category.component';
import { ViewCategoryComponent } from '../view-category/view-category.component';
import { CreateCategory } from '../../../../models/product-models/createCategory';
import { Category } from '../../../../models/product-models/category';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-manage-category',
  standalone: true,
  imports: [NgIf,CreateCategoryComponent,ViewCategoryComponent, CommonModule, FormsModule],
  templateUrl: './manage-category.component.html',
  styleUrl: './manage-category.component.scss'
})
export class ManageCategoryComponent implements OnInit{
  newCategory ?: CreateCategory | null;
  viewCategory ?: Category | null;
  categories : Category[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllCategories();
  }

  getAllCategories() {
    this.getService.getAllCategoriesAsync().subscribe({
      next: res => {
        this.categories = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  openCreateCategoryForm() {
    this.newCategory= new CreateCategory;
  }

  closeCreateCategoryForm() {
    this.newCategory = null;
    this.getAllCategories();
  }

  openViewCategoryForm() {
    this.viewCategory= new Category;
  }
}
