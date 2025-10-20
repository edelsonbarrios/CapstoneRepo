import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

// Interfaces for table data
interface TopSellingMedicine {
  rank: number;
  productName: string;
  soldCount: string;
}

interface ExpiringProduct {
  batchNo: string;
  product: string;
  expiry: string;
}

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [RouterLink, NgFor, NgIf],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent implements OnInit {
  // Top Selling Medicines data
  topSellingMedicines: TopSellingMedicine[] = [
    { rank: 1, productName: 'Biogesic 500mg', soldCount: '320 sold' },
    { rank: 2, productName: 'Cecon 500mg', soldCount: '280 sold' },
    { rank: 3, productName: 'Bioflu 500mg', soldCount: '250 sold' },
    { rank: 4, productName: 'Amoxicillin 500mg', soldCount: '200 sold' },
    { rank: 5, productName: 'Cetirizine 10mg', soldCount: '180 sold' },
    { rank: 6, productName: 'Paracetamol 500mg', soldCount: '175 sold' },
    { rank: 7, productName: 'Ibuprofen 400mg', soldCount: '160 sold' },
    { rank: 8, productName: 'Mefenamic Acid 500mg', soldCount: '150 sold' }
  ];
  paginatedTopMeds: TopSellingMedicine[] = [];

  // Expiring Soon data
  expiringProducts: ExpiringProduct[] = [
    { batchNo: 'AMOX–091523–A', product: 'Amoxicillin 500mg', expiry: 'Sep 28, 2025' },
    { batchNo: 'BFL-100823-B', product: 'Bioflu 500mg', expiry: 'Oct 10, 2025' },
    { batchNo: 'BIO-101323-01', product: 'Biogesic 500mg', expiry: 'Oct 15, 2025' },
    { batchNo: 'CET-102223-03', product: 'Cetirizine 10mg', expiry: 'Oct 25, 2025' },
    { batchNo: 'PAR-110123-02', product: 'Paracetamol 500mg', expiry: 'Nov 5, 2025' },
    { batchNo: 'IBU-111523-04', product: 'Ibuprofen 400mg', expiry: 'Nov 12, 2025' },
    { batchNo: 'MEF-112023-05', product: 'Mefenamic Acid 500mg', expiry: 'Nov 20, 2025' }
  ];
  paginatedExpiringProducts: ExpiringProduct[] = [];

  // Pagination properties for Top Selling Medicines
  topMedsCurrentPage: number = 1;
  topMedsItemsPerPage: number = 2;
  topMedsTotalPages: number = 0;

  // Pagination properties for Expiring Soon
  expiryCurrentPage: number = 1;
  expiryItemsPerPage: number = 3;
  expiryTotalPages: number = 0;

  ngOnInit(): void {
    this.updateTopMedsPagination();
    this.updateExpiryPagination();
  }

  // Top Selling Medicines Pagination
  updateTopMedsPagination() {
    this.topMedsTotalPages = Math.ceil(this.topSellingMedicines.length / this.topMedsItemsPerPage);
    if (this.topMedsTotalPages === 0) this.topMedsTotalPages = 1;
    const startIndex = (this.topMedsCurrentPage - 1) * this.topMedsItemsPerPage;
    const endIndex = startIndex + this.topMedsItemsPerPage;
    this.paginatedTopMeds = this.topSellingMedicines.slice(startIndex, endIndex);
  }

  goToTopMedsPage(page: number) {
    if (page >= 1 && page <= this.topMedsTotalPages) {
      this.topMedsCurrentPage = page;
      this.updateTopMedsPagination();
    }
  }

  // Expiring Soon Pagination
  updateExpiryPagination() {
    this.expiryTotalPages = Math.ceil(this.expiringProducts.length / this.expiryItemsPerPage);
    if (this.expiryTotalPages === 0) this.expiryTotalPages = 1;
    const startIndex = (this.expiryCurrentPage - 1) * this.expiryItemsPerPage;
    const endIndex = startIndex + this.expiryItemsPerPage;
    this.paginatedExpiringProducts = this.expiringProducts.slice(startIndex, endIndex);
  }

  goToExpiryPage(page: number) {
    if (page >= 1 && page <= this.expiryTotalPages) {
      this.expiryCurrentPage = page;
      this.updateExpiryPagination();
    }
  }
}