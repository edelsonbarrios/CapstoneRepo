import { Component, OnInit } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { StockAdjustComponent } from '../stock-adjust/stock-adjust.component';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';
import { ViewAdjustmentComponent } from '../view-adjustment/view-adjustment.component';

@Component({
  selector: 'app-manage-stock-adjustment',
  standalone: true,
  imports: [
    StockAdjustComponent,
    NgIf, 
    CommonModule, 
    FormsModule,
    ViewAdjustmentComponent,
  ],
  templateUrl: './manage-stock-adjustment.component.html',
  styleUrl: './manage-stock-adjustment.component.scss'
})
export class ManageStockAdjustmentComponent implements OnInit {
  stockAdjust?: StockMovement | null;
  viewAdjust?: StockMovement | null;
  stockMovements: StockMovement[] = [];
  paginatedStockMovements: StockMovement[] = [];
  searchQuery: string = '';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService
  ) {}

  ngOnInit(): void {
    this.getAllStockMovements();
  }

  get filteredStockMovements(): StockMovement[] {
    let movements = this.stockMovements;
    
    // ✅ FILTER OUT specific movement types
  const excludedTypes = ['sale', 'damaged', 'expired', 'count correction'];
  
  movements = movements.filter(sm => {
    const type = sm.movementType.toLowerCase();
    return !excludedTypes.includes(type);
  });
  
    // Apply search filter
    if (!this.searchQuery.trim()) {
      return movements;
    }
    
    const query = this.searchQuery.toLowerCase();
    return movements.filter(sm => 
      sm.productName.toLowerCase().includes(query) ||
      sm.referenceNumber.toLowerCase().includes(query)
    );
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
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
    const items = this.filteredStockMovements;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedStockMovements = items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openStockAdjustmentForm(stockMovement: StockMovement) {
    this.stockAdjust = stockMovement;
  }

  openViewAdjustmentForm(request: StockMovement) {
    this.viewAdjust = request;
  }

  closeStockAdjustmentForm() {
    this.stockAdjust = null;
    this.getAllStockMovements();
  }

  closeViewAdjustmentForm() {
    this.viewAdjust = null;
  }
}