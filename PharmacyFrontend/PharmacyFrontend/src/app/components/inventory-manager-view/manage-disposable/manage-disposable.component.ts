import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { Product } from '../../../models/product-models/product';
import { Category } from '../../../models/category-models/category';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { Brand } from '../../../models/brand-models/brand';
import { Generic } from '../../../models/generic-models/generic';
import { UpdateService } from '../../../services/UpdateService/update.service';
import { RestoreConfirmationComponent } from '../../shared/restore-confirmation/restore-confirmation.component';
import { DeleteConfirmationComponent } from '../../shared/delete-confirmation/delete-confirmation.component';

@Component({
  selector: 'app-manage-disposable',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RestoreConfirmationComponent,
    DeleteConfirmationComponent
  ],
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
  
  // Restore confirmation modal states
  showRestoreConfirmation: boolean = false;
  restoreMessage: string = '';
  restoreAlertMessage: string = '';
  itemToRestore: any = null;

  // Delete confirmation modal states
  showDeleteConfirmation: boolean = false;
  deleteMessage: string = '';
  deleteAlertMessage: string = '';
  itemToDelete: any = null;

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

  constructor(
    private getService: GetService,
    private updateService: UpdateService,
  ) {}

  ngOnInit(): void {
    this.getAllItems();
  }

  getAllItems() {
    this.all = [];
    this.getAllProducts();
    this.getAllCategories();
    this.getInventoriesOutOfStock();
    this.getAllInactiveBrands();
    this.getAllInactiveGenerics();
    this.getAllInactiveFormulations();
    this.getAllInactiveClassifications();
    this.getAllInactiveTypes();
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

  getAllInactiveClassifications() {
    this.getService.getAllInActiveClassificationsAsync().subscribe({
      next: res => {
        const classificationsWithType = res.map(c => ({...c, entityType: 'classification'}));
        this.all.push(...classificationsWithType);
        this.updatePaginatedItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    })
  }

  getAllInactiveTypes() {
    this.getService.getAllInactiveTypesAsync().subscribe({
      next: res => {
        const typesWithType = res.map(t => ({...t, entityType: 'type' }));
        this.all.push(...typesWithType);
        this.updatePaginatedItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    })
  }

  updatePaginatedItems() {
    const items = this.filteredItems;

    const sortedItems = items.sort((a, b) => {
      const typeOrder = ['product', 'category', 'inventory', 'brand', 'generic', 'formulation', 'classification', 'type'];
      const indexA = typeOrder.indexOf(a.entityType);
      const indexB = typeOrder.indexOf(b.entityType);
      return indexA - indexB;
    });

    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages == 0) this.totalPages = 1;

    const startingIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endingIndex = startingIndex + this.itemsPerPage;
    this.paginatedItems = sortedItems.slice(startingIndex, endingIndex);
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
    } else if (item.entityType === 'inventory') {
      return item.productName;
    } else if (item.entityType === 'brand') {
      return item.brandName;
    } else if (item.entityType === 'generic') {
      return item.genericName;
    } else if (item.entityType === 'formulation') {
      return item.formulationName;
    } else if (item.entityType === 'classification') {
      return item.classificationName;
    } else if (item.entityType === 'type') {
      return item.typeName;
    }
    return 'Unknown';
  }

  getItemType(item: any): string {
    if (item.entityType === 'product') return 'Product';
    if (item.entityType === 'category') return 'Category';
    if (item.entityType === 'inventory') return 'Inventory';
    if (item.entityType === 'brand') return 'Brand';
    if (item.entityType === 'generic') return 'Generic';
    if (item.entityType === 'formulation') return 'Formulation';
    if (item.entityType === 'classification') return 'Classification';
    if (item.entityType === 'type') return 'Type';
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

  restoreItem(item: any) {
    // Store the item and show confirmation modal
    this.itemToRestore = item;
    this.restoreMessage = `Are you sure you want to restore this ${this.getItemType(item)}?`;
    this.restoreAlertMessage = `${this.getItemType(item)} has been restored successfully!`;
    this.showRestoreConfirmation = true;
  }

  onConfirmRestore() {
    // This is called when user clicks "OK" on success alert
    if (!this.itemToRestore) return;

    const item = this.itemToRestore;

    switch (item.entityType) {
      case "category":
        this.restoreCategoryById(item.categoryID);
        break;
      case "product":
        this.restoreProductById(item.productID);
        break;
      case "brand":
        this.restoreBrandById(item.brandID);
        break;
      case "generic":
        this.restoreGenericById(item.genericID);
        break;
      case "formulation":
        this.restoreFormulationById(item.formulationID);
        break;
      case "classification":
        this.restoreClassificationById(item.classificationID);
        break;
      case "type":
        this.restoreTypeModelById(item.typeID);
        break;
    }

    // Close the modal and reset
    this.showRestoreConfirmation = false;
    this.itemToRestore = null;
    this.getAllItems();
  }

  onCancelRestore() {
    // User clicked cancel
    this.showRestoreConfirmation = false;
    this.itemToRestore = null;
  }

  archiveItem(item: any) {
    // Store the item and show delete confirmation modal
    this.itemToDelete = item;
    this.deleteMessage = `Are you sure you want to delete this ${this.getItemType(item)}?`;
    this.deleteAlertMessage = `${this.getItemType(item)} has been deleted successfully!`;
    this.showDeleteConfirmation = true;
  }

  onConfirmDelete() {
    // This is called when user clicks "OK" on success alert
    if (!this.itemToDelete) return;

    this.deleteInventory(this.itemToDelete.inventoryID);

    // Close the modal and reset
    this.showDeleteConfirmation = false;
    this.itemToDelete = null;
  }

  onCancelDelete() {
    // User clicked cancel
    this.showDeleteConfirmation = false;
    this.itemToDelete = null;
  }

  restoreTypeModelById(id: number) {
    this.updateService.restoreTypeModelByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreClassificationById(id: number) {
    this.updateService.restoreClassificationByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreFormulationById(id: number) {
    this.updateService.restoreFormulationByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreGenericById(id: number) {
    this.updateService.restoreGenericByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreBrandById(id: number) {
    this.updateService.restoreBrandByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreProductById(id: number) {
    this.updateService.restoreProductAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  restoreCategoryById(id: number) {
    this.updateService.restoreCategoryByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    })
  }

  deleteInventory(id: number) {
    this.updateService.deleteInventoryByIdAsync(id).subscribe({
      next: res => {
        this.getAllItems();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    })
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