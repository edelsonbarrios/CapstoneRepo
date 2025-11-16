import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { Product } from '../../../models/product-models/product';
import { Category } from '../../../models/category-models/category';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { Brand } from '../../../models/brand-models/brand';
import { Generic } from '../../../models/generic-models/generic';


@Component({
  selector: 'app-manage-disposable',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './manage-disposable.component.html',
  styleUrl: './manage-disposable.component.scss'
})
export class ManageDisposableComponent implements OnInit {
  products: Product[] = [];
  categories: Category[] = [];
  all: any[] = [];
  paginatedItems: any[] = [];
  itemsPerPage: number = 5;
  totalPages: number = 0;
  currentPage: number = 1;
  searchQuery: string = '';
  inventories: Inventory[] = [];
  brands: Brand[] = [];
  generics: Generic[] = [];

  Math = Math;

  // Modal states
  showModal: boolean = false;
  showSuccessAlert: boolean = false;
  isEditMode: boolean = false;

  // Form data
  formData = {
    id: null,
    name: '',
    entityType: 'product'
  };

  get filteredItems(): any[] {
    if (this.searchQuery.trim() == '') {
      return this.all;
    }
    const query = this.searchQuery.toLowerCase();
    return this.all.filter(item => {
      return (
        item.productName?.toLowerCase().includes(query) ||
        item.categoryName?.toLowerCase().includes(query)
      );
    });
  }

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getAllItems();
  }

  getAllItems() {
    this.getAllProducts();
    this.getAllCategories();
    this.getInventoriesOutOfStock();
    this.getAllInactiveBrands();
    this.getAllInactiveGenerics();
    this.getAllInactiveFormulations();
    this.getAllInactiveFormulations();
  }

  getAllProducts() {
    this.getService.getAllInactiveProductsAsync().subscribe({
      next: res => {
        this.products = res;
        const productsWithType = res.map(p => ({ ...p, entityType: 'product' }));
        this.all.push(...productsWithType);
        this.updatePaginatedItems();
      },
      error: err => {
        console.log(`${err.error}`);
        console.log(`${this.products}`);
      }
    });
  }

  getAllCategories() {
    this.getService.getAllInactiveCategoriesAsync().subscribe({
      next: res => {
        this.categories = res;
        const categoriesWithType = res.map(c => ({ ...c, entityType: 'category' }));
        this.all.push(...categoriesWithType);
        this.updatePaginatedItems();
      },
      error: err => {
        console.log(`${err.error}`);
        console.log(`${this.categories}`);
      }
    });
  }

  getInventoriesOutOfStock() {
    this.getService.getInventoriesOutOfStockAsync().subscribe({
      next: res => {
        this.inventories = res;
        const inventoriesWithType = res.map(iv => ({...iv, entityType: 'inventory'}));
        this.all.push(...inventoriesWithType);
        this.updatePaginatedItems();
      }
    })
  }

  getAllInactiveBrands() {
    this.getService.getAllInactiveBrandsAsync().subscribe({
      next: res => {
        const brandsWithType = res.map(b => ({...b, entityType: 'brand'}));
        this.all.push(...brandsWithType);
        this.updatePaginatedItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    })
  }

  getAllInactiveGenerics() {
    this.getService.getAllInactiveGenericsAsync().subscribe({
      next: res => {
        const genericsWithType = res.map(g => ({...g, entityType: 'generic'}));
        this.all.push(...genericsWithType);
        this.updatePaginatedItems();
      }
    })
  }

  getAllInactiveFormulations() {
    this.getService.getAllInactiveFormulationsAsync().subscribe({
      next: res => {
        const formulationsWithType = res.map(f => ({...f, entityType: 'formulation'}));
        this.all.push(...formulationsWithType);
        this.updatePaginatedItems();
      }
    })
  }

  updatePaginatedItems() {
    const items = this.filteredItems;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages == 0) this.totalPages = 1;

    const startingIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endingIndex = startingIndex + this.itemsPerPage;
    this.paginatedItems = items.slice(startingIndex, endingIndex);
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePaginatedItems();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedItems();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedItems();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedItems();
    }
  }

  getItemName(item: any): string {
    if (item.entityType === 'product') {
      return item.productName;
    } else if (item.entityType === 'category') {
      return item.categoryName;
    }else if(item.entityType === 'inventory') {
      return item.productName;
    }else if(item.entityType === 'brand') {
      return item.brandName;
    }else if(item.entityType === 'generic') {
      return item.genericName;
    }else if(item.entityType === 'formulation') {
      return item.formulationName;
    }
    return 'Unknown';
  }

  getItemType(item: any): string {
    if(item.entityType === 'product') return 'Product';
    if(item.entityType === 'category') return 'Category';
    if(item.entityType === 'inventory') return 'Inventory';
    if(item.entityType === 'brand') return 'Brand';
    if(item.entityType === 'generic') return 'Generic';
    if(item.entityType === 'formulation') return 'Formulation';
    return 'Unknown';
  }

  openAddModal() {
    this.isEditMode = false;
    this.formData = {
      id: null,
      name: '',
      entityType: 'product'
    };
    this.showModal = true;
  }

  editItem(item: any) {
    this.isEditMode = true;
    this.formData = {
      id: item.id || item.productId || item.categoryId,
      name: this.getItemName(item),
      entityType: item.entityType
    };
    this.showModal = true;
  }

  deleteItem(item: any) {
    if (confirm(`Are you sure you want to delete ${this.getItemName(item)}?`)) {
      console.log('Delete item:', item);
      // Implement delete logic here
      // After successful delete:
      // this.all = this.all.filter(i => i !== item);
      // this.updatePaginatedItems();
    }
  }

  closeModal() {
    this.showModal = false;
    this.formData = {
      id: null,
      name: '',
      entityType: 'product'
    };
  }

  saveItem() {
    if (!this.formData.name.trim()) {
      return;
    }

    console.log('Save item:', this.formData);

    // Implement save logic here
    // After successful save:
    this.showModal = false;
    this.showSuccessAlert = true;

    // Refresh data
    // this.getAllProducts();
    // this.getAllCategories();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
  }

  get pageNumbers(): number[] {
    const maxVisible = 5;
    const pages: number[] = [];

    if (this.totalPages <= maxVisible) {
      return Array.from({ length: this.totalPages }, (_, i) => i + 1);
    }

    let start = Math.max(1, this.currentPage - 2);
    let end = Math.min(this.totalPages, start + maxVisible - 1);

    if (end - start < maxVisible - 1) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }
}
