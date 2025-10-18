import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { AssignProductSupplierRequest } from '../../../../models/supplier-models/assign-product-supplier';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-assign-products-supplier',
  standalone: true,
  imports: [CurrencyPipe, FormsModule, NgFor, NgIf],
  templateUrl: './assign-products-supplier.component.html',
  styleUrl: './assign-products-supplier.component.scss'
})
export class AssignProductsSupplierComponent implements OnInit {
  @Input() supplier: Supplier = new Supplier();
  @Output() close = new EventEmitter<void>();
  
  products: Product[] = [];
  showSuccessAlert: boolean = false;
  searchQuery: string = '';
  
  // Pagination
  paginatedProducts: Product[] = [];
  currentPage = 1;
  itemsPerPage = 8;
  totalPages = 0;

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  // Filtered products based on search query
  get filteredProducts(): Product[] {
    if (!this.searchQuery.trim()) {
      return this.products;
    }
    const query = this.searchQuery.toLowerCase();
    return this.products.filter(product =>
      product.productName.toLowerCase().includes(query) ||
      product.categoryName?.toLowerCase().includes(query) ||
      product.brandName?.toLowerCase().includes(query) ||
      product.genericName?.toLowerCase().includes(query)
    );
  }

  updatePagination() {
    const items = this.filteredProducts;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;

    // Reset to page 1 if current page exceeds total pages
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = items.slice(startIndex, endIndex);
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  toggleAll(event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    this.paginatedProducts.forEach(p => (p.selected = checked));
  }

  get selectedCount(): number {
    return this.products.filter(p => p.selected).length;
  }

  getAllProducts() {
    this.getService.getAllProductsAsync().subscribe({
      next: res => {
        this.products = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  assignProducts() {
    if (!this.supplier || this.products.length == 0) return;

    const selectedProducts = this.products.filter(p => p.selected);

    if (selectedProducts.length === 0) {
      alert('Please select at least one product.');
      return;
    }

    const requestPayload: AssignProductSupplierRequest = {
      supplierId: this.supplier.id,
      productsToBeAdded: selectedProducts
    };

    this.updateService.assignProductsToSupplierAsync(requestPayload).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        alert(`${err.error}`);
        console.log(`${err.error}`);
        console.log(`${err.error.message}`);
      }
    });
  }

  cancel() {
    this.close.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.close.emit();
  }
}