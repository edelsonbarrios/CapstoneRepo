import { Component, OnInit } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-fast-moving-products',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './fast-moving-products.component.html',
  styleUrl: './fast-moving-products.component.scss'
})
export class FastMovingProductsComponent implements OnInit {
  products: any[] = [];
  paginatedProducts: any[] = [];

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  // Modal
  showExportModal: boolean = false;

  ngOnInit(): void {
    // Call your method to populate products array here
    // Example: this.getAllProducts();
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.products.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  showExportConfirmation(): void {
    this.showExportModal = true;
  }

  confirmExport(): void {
    this.showExportModal = false;
    this.exportToExcel();
  }

  cancelExport(): void {
    this.showExportModal = false;
  }

  exportToExcel(): void {
    // TODO: Implement export logic here
    console.log('Export to Excel functionality to be implemented');
    alert('Export functionality will be implemented here');
  }
}