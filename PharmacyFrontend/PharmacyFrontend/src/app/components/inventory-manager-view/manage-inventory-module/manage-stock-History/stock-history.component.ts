import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface StockHistorySummary {
  totalTransactions: number;
  stockEntries: number;
  adjustments: number;
  salesRemovals: number;
}

interface StockHistoryTransaction {
  stockMovementID: number;
  movementDate: string;
  productName: string;
  batchNumber: string;
  movementType: string;
  referenceNumber?: string;
  boxQuantityBefore: number;
  sheetQuantityBefore: number;
  pieceQuantityBefore: number;
  boxQuantityChanged: number;
  sheetQuantityChanged: number;
  pieceQuantityChanged: number;
  boxQuantityAfter: number;
  sheetQuantityAfter: number;
  pieceQuantityAfter: number;
  performedBy: string;
  notes?: string;
}

interface StockHistoryResponse {
  summary: StockHistorySummary;
  transactions: StockHistoryTransaction[];
}

@Component({
  selector: 'app-Manage-stock-history',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './stock-history.component.html',
  styleUrl: './stock-history.component.scss'
})
export class ManageStockHistoryComponent implements OnInit {
  startDate: string = '';
  endDate: string = '';
  selectedTransactionType: string = '';
  searchTerm: string = '';

  stockHistoryResponse: StockHistoryResponse = {
    summary: {
      totalTransactions: 0,
      stockEntries: 0,
      adjustments: 0,
      salesRemovals: 0
    },
    transactions: []
  };

  transactionTypes: string[] = [
    'Stock Entry',
    'Count Correction',
    'Sale',
    'Damaged',
    'Expired'
  ];

  loading: boolean = false;
  error: string = '';

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;
  paginatedTransactions: StockHistoryTransaction[] = [];

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getStockHistory();
  }

  getStockHistory(): void {
    const requestPayload: any = {
      startDate: this.startDate
        ? new Date(this.startDate).toISOString()
        : undefined,
      endDate: this.endDate ? new Date(this.endDate).toISOString() : undefined,
      transactionType: this.selectedTransactionType || undefined,
      searchTerm: this.searchTerm || undefined
    };

    this.loading = true;
    this.error = '';

    this.getService
      .getStockHistory(
        requestPayload.startDate,
        requestPayload.endDate,
        requestPayload.transactionType,
        requestPayload.searchTerm
      )
      .subscribe({
        next: (res: StockHistoryResponse) => {
          console.log('Stock History Response:', res);
          this.stockHistoryResponse = res;
          this.loading = false;
          this.setupPagination();
        },
        error: (err) => {
          console.error('Error fetching stock history:', err);
          this.error = err.error?.message || 'Failed to load stock history';
          this.loading = false;
          this.stockHistoryResponse = {
            summary: {
              totalTransactions: 0,
              stockEntries: 0,
              adjustments: 0,
              salesRemovals: 0
            },
            transactions: []
          };
        }
      });
  }

  setupPagination(): void {
    const items = this.stockHistoryResponse.transactions || [];
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    this.currentPage = 1;
    this.updatePaginatedData();
  }

  resetFilters(): void {
    this.startDate = '';
    this.endDate = '';
    this.selectedTransactionType = '';
    this.searchTerm = '';
    this.onFilterChange();
  }

  updatePaginatedData(): void {
    const items = this.stockHistoryResponse.transactions || [];
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedTransactions = items.slice(start, end);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.getStockHistory();
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.getStockHistory();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
  }

  // Helper method for formatting +/- signs in Changed column
  formatChange(value: number): string {
    if (!value || value === 0) return '0';
    return value > 0 ? `+${value}` : `${value}`;
  }

  getMovementTypeClass(movementType: string): string {
    const type = movementType.toLowerCase();
    if (type === 'stock in' || type === 'stock entry') return 'badge-success';
    if (type === 'sale') return 'badge-primary';
    if (type === 'Count Correction') return 'badge-warning';
    if (type === 'damaged' || type === 'expired') return 'badge-danger';
    return 'badge-secondary';
  }

  getMovementTypeDisplay(movementType: string): string {
    return movementType === 'Stock In' ? 'Stock Entry' : movementType;
  }
}
