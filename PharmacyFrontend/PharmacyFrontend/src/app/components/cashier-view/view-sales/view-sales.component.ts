import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Product } from '../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';

// Item interface for the items table
interface SaleItem {
  product: string;
  unit: string;
  qty: number;
  price: number;
  subtotal: number;
}

@Component({
  selector: 'app-view-sales',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './view-sales.component.html',
  styleUrl: './view-sales.component.scss'
})
export class ViewSalesComponent implements OnInit, OnChanges {
  @Input() viewSales: Product = new Product();
  @Input() items: SaleItem[] = [];
  paginatedItems: SaleItem[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  ngOnInit(): void {
    this.updatePagination();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items']) {
      this.currentPage = 1;
      this.updatePagination();
    }
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }
}