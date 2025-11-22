import { Component, Output, EventEmitter, OnInit, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { GetService } from '../../../../services/GetService/get.service';

@Component({
  selector: 'app-purchase-order-products',
  standalone: true,
  imports: [FormsModule, CurrencyPipe, NgFor, NgIf],
  templateUrl: './purchase-order-products.component.html',
  styleUrl: './purchase-order-products.component.scss'
})
export class PurchaseOrderProductsComponent implements OnInit {
  @Input() supplierProudcts: Product[] = [];
  @Output() productsSelected = new EventEmitter<Product[]>();
  @Output() close = new EventEmitter<void>();

  products: Product[] = [];
  searchQuery: string = '';
  addItem: boolean = false;

  // Pagination
  paginatedProducts: Product[] = [];
  currentPage = 1;
  itemsPerPage = 8;
  totalPages = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.updatePagination();

    this.supplierProudcts.forEach(p => {
      this.initializeProductUnitOrderType(p);
    });
  }

  // Filtered products based on search query
  get filteredProducts(): Product[] {
    if (!this.searchQuery.trim()) {
      return this.supplierProudcts;
    }
    const query = this.searchQuery.toLowerCase();
    return this.supplierProudcts.filter(product =>
      product.productName.toLowerCase().includes(query) ||
      product.categoryName?.toLowerCase().includes(query) ||
      product.brandName?.toLowerCase().includes(query) ||
      product.genericName?.toLowerCase().includes(query)
    );
  }

  initializeProductUnitOrderType(prod : Product) : void {
    if(prod.unitMeasurement == 'Multi-Level' || prod.unitMeasurement == 'Dual-Level') {
      prod.poUnitType = 'Box';
    } else {
      prod.poUnitType = 'Pieces';
    }
  }

  getUnitPrice(prod: Product) : number{
    return prod.poUnitType == 'Box' ? prod.boxPrice : prod.piecePrice;
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
    return this.supplierProudcts.filter(p => p.selected).length;
  }

  addSelected() {
    const selected = this.supplierProudcts.filter(p => p.selected);

    if (selected.length === 0) {
      this.addItem = true;
      return;
    }

    this.productsSelected.emit(selected);
  }

  closeAddItemMessage() {
    this.addItem = false;
  }

  cancel() {
    this.close.emit();
  }
}
