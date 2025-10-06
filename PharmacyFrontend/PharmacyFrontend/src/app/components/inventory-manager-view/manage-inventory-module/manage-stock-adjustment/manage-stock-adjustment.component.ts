import { Component, OnInit } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { StockAdjustComponent } from '../stock-adjust/stock-adjust.component';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';

@Component({
  selector: 'app-manage-stock-adjustment',
  standalone: true,
  imports: [StockAdjustComponent,
    NgIf, 
    CommonModule, 
    FormsModule,
  ],
  templateUrl: './manage-stock-adjustment.component.html',
  styleUrl: './manage-stock-adjustment.component.scss'
})
export class ManageStockAdjustmentComponent implements OnInit{
  stockAdjust ?: StockMovement | null;
  stockMovements: StockMovement[] = [];
  paginatedStockMovements: StockMovement[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllStockMovements();
  }

  getAllStockMovements() {
    this.getService.getAllStockMovementsAsync().subscribe({
      next: res => {
        this.stockMovements = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.stockMovements.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedStockMovements = this.stockMovements.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openStockAdjustmentForm(stockMovement : StockMovement) {
    this.stockAdjust = stockMovement;
  }

  closeStockAdjustmentForm() {
    this.stockAdjust = null;
  }
}