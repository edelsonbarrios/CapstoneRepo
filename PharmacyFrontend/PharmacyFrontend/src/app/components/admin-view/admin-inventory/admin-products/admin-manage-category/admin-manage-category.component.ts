import { Component, OnInit } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateCategoryComponent } from '../../../../inventory-manager-view/inventory-manager-product/create-category/create-category.component';
import { ViewCategoryComponent } from '../../../../inventory-manager-view/inventory-manager-product/view-category/view-category.component';
import { CreateCategory } from '../../../../../models/product-models/createCategory';
import { Category } from '../../../../../models/category-models/category';
import { GetService } from '../../../../../services/GetService/get.service';
import { UpdateService } from '../../../../../services/UpdateService/update.service';
import { ArchiveConfirmationComponent } from '../../../../shared/archive-confirmation/archive-confirmation.component';

@Component({
  selector: 'app-manage-category',
  standalone: true,
  imports: [
    NgIf,
    CreateCategoryComponent,
    ViewCategoryComponent, CommonModule,
    FormsModule,
    ArchiveConfirmationComponent,
  ],
  templateUrl: './admin-manage-category.component.html',
  styleUrl: './admin-manage-category.component.scss'
})
export class ManageCategoryComponent implements OnInit {
  newCategory?: CreateCategory | null;
  viewCategory?: Category | null;
  categories: Category[] = [];
  paginatedCategories: Category[] = [];
  showSuccessAlert: boolean = false;
  categoryToUpdate?: Category | null;
  showArchiveForm: number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  searchQuery: string = '';

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) { }

  ngOnInit(): void {
    this.getAllCategories();
  }

  getAllCategories() {
    this.getService.getAllCategoriesAsync().subscribe({
      next: res => {
        this.categories = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  get filteredCategories(): Category[] {
    if (!this.searchQuery.trim()) return this.categories;
    const query = this.searchQuery.toLowerCase();
    return this.categories.filter(c => c.categoryName.toLowerCase().includes(query));
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    var categories = this.filteredCategories;
    this.totalPages = Math.ceil(categories.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedCategories = categories.slice(startIndex, endIndex);
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveCategory() {
    this.updateService.archiveCategoryByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        if (!this.showArchiveForm) return;
        this.showArchiveForm = 0;
        this.getAllCategories();
      }
    })
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openCreateCategoryForm() {
    this.newCategory = new CreateCategory;
  }

  closeCreateCategoryForm() {
    this.newCategory = null;
    this.getAllCategories();
  }

  openViewCategoryForm(cat: Category) {
    this.categoryToUpdate = cat;
  }

  closeUpdateCategoryForm() {
    this.categoryToUpdate = null;
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.currentPage = 1;
    this.getAllCategories();
  }
}