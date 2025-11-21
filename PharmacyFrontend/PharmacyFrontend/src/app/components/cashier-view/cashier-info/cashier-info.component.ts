import { Component, OnInit } from '@angular/core';
import { NgFor, NgIf, CommonModule } from '@angular/common';
import { GetService } from '../../../services/GetService/get.service';
import { ProductSalesSummary } from '../../../models/sales-transaction-models/product-sales-summary';

// Extended interface for top selling medicines with unit breakdown
interface TopSellingMedWithBreakdown extends ProductSalesSummary {
  unitType: 'Multi-Level' | 'Dual-Level' | 'Single-Level';
  quantitySoldBreakdown: {
    box?: number;
    sheet?: number;
    piece: number;
  };
  totalQty: number;
  boxSold?: number;
  sheetSold?: number;
  pieceSold?: number;
  unitMeasurement?: string;
}

@Component({
  selector: 'app-cashier-info',
  standalone: true,
  imports: [NgFor, NgIf, CommonModule],
  templateUrl: './cashier-info.component.html',
  styleUrl: './cashier-info.component.scss'
})
export class CashierInfoComponent implements OnInit {
  topSellingMedicines: TopSellingMedWithBreakdown[] = [];
  paginatedTopMeds: TopSellingMedWithBreakdown[] = [];
  
  topMedsCurrentPage: number = 1;
  topMedsItemsPerPage: number = 5;
  topMedsTotalPages: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getTopSellingProductsMonthly();
  }

  getTopSellingProductsMonthly() {
    this.getService.getTopSellingProductsMonthly().subscribe({
      next: res => {
        console.log('Backend response:', res);
        
        this.topSellingMedicines = res.map(product => {
          const mapped: TopSellingMedWithBreakdown = {
            ...product,
            unitType: (product.unitMeasurement || 'Single-Level') as 'Multi-Level' | 'Dual-Level' | 'Single-Level',
            quantitySoldBreakdown: {
              box: product.boxSold ?? undefined,
              sheet: product.sheetSold ?? undefined,
              piece: product.pieceSold ?? product.quantitySold ?? 0
            },
            totalQty: product.quantitySold || 0,
            boxSold: product.boxSold,
            sheetSold: product.sheetSold,
            pieceSold: product.pieceSold,
            unitMeasurement: product.unitMeasurement
          };
          
          console.log('Mapped product:', mapped);
          return mapped;
        });
        
        this.updateTopMedsPagination();
      },
      error: err => console.log(`${err.error}`)
    });
  }

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
}