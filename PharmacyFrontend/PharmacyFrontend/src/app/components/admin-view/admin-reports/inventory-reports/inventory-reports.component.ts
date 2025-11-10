import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { InventoryReport } from '../../../../models/report-models/inventory-report';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-inventory-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './inventory-reports.component.html',
  styleUrl: './inventory-reports.component.scss',
})
export class InventoryReportsComponent implements OnInit {
  startDate: string = '';
  endDate: string = '';
  selectedFilterType: string = '';
  inventoryReport: InventoryReport = new InventoryReport();

  filterTypes: string[] = ['Critical Stock', 'Near Expiry', 'Expired'];

  loading: boolean = false;
  error: string = '';
  exporting: boolean = false;

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;
  paginatedInventory: any[] = [];

  // Modal
  showExportModal: boolean = false;
  showValidationModal: boolean = false;
  validationMessage: string = '';

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getInventoryReport();
  }

  getInventoryReport(): void {
    const requestPayload: any = {
      startDate: this.startDate ? new Date(this.startDate).toISOString() : undefined,
      endDate: this.endDate ? new Date(this.endDate).toISOString() : undefined,
      filterType: this.selectedFilterType || undefined,
    };

    this.loading = true;
    this.getService
      .getInventoryReportsAsync(
        requestPayload.startDate,
        requestPayload.endDate,
        requestPayload.filterType
      )
      .subscribe({
        next: (res) => {
          this.inventoryReport = res;
          this.loading = false;
          this.setupPagination();
        },
        error: (err) => {
          this.error = err.error;
          this.loading = false;
        },
      });
  }

  setupPagination(): void {
    const items = this.inventoryReport.inventoryItems || [];
    
    // Sort by dateAdded in descending order (latest first)
    const sortedItems = items.sort((a: any, b: any) => {
      const dateA = new Date(a.dateAdded).getTime();
      const dateB = new Date(b.dateAdded).getTime();
      return dateB - dateA;
    });
    
    this.inventoryReport.inventoryItems = sortedItems;
    this.totalPages = Math.ceil(sortedItems.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    this.updatePaginatedData();
  }

  resetFilters(): void {
    this.startDate = '';
    this.endDate = '';
    this.selectedFilterType = '';
    this.onFilterChange();
  }

  updatePaginatedData(): void {
    const items = this.inventoryReport.inventoryItems || [];
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedInventory = items.slice(start, end);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.getInventoryReport();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
  }

  showExportConfirmation(): void {
    if (!this.startDate || !this.endDate) {
      this.validationMessage = 'Please select both Start Date and End Date before exporting.';
      this.showValidationModal = true;
      return;
    }

    if (new Date(this.startDate) > new Date(this.endDate)) {
      this.validationMessage = 'Start Date cannot be later than End Date.';
      this.showValidationModal = true;
      return;
    }
    
    this.showExportModal = true;
  }

  closeValidationModal(): void {
    this.showValidationModal = false;
  }

  confirmExport(): void {
    this.showExportModal = false;
    this.exportToExcel();
  }

  cancelExport(): void {
    this.showExportModal = false;
  }

  exportToExcel(): void {
    if (this.exporting) return;

    this.exporting = true;

    const start = this.startDate ? new Date(this.startDate) : undefined;
    const end = this.endDate ? new Date(this.endDate) : undefined;
    const filterType = this.selectedFilterType || undefined;

    this.getService.exportInventoryReport(start, end, filterType).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        const filterSuffix = this.selectedFilterType ? `_${this.selectedFilterType}` : '';
        const fileName = `InventoryReport_${this.startDate}_${this.endDate}${filterSuffix}.xlsx`;
        link.download = fileName;
        
        link.click();
        window.URL.revokeObjectURL(url);
        this.exporting = false;
      },
      error: (err) => {
        console.error('Error exporting inventory report:', err);
        alert('Failed to export report. Please try again.');
        this.exporting = false;
      }
    });
  }

  getInventoryStatus(inv: any): string {
    if (inv.isExpired) return 'Expired';
    if (inv.isNearExpiry) return 'Near Expiry';
    if (inv.isCriticalStock) return 'Critical Stock';
    return 'Normal';
  }

  getUnitMeasurement(inv: any): string {
    // Priority 1: Use explicit unitMeasurement from product if available
    if (inv.product?.unitMeasurement) {
      return inv.product.unitMeasurement;
    }
    
    const invAny = inv as any;
    const productAny = inv.product as any;
    
    // Extract conversion values from inventory or product
    const sheetsPerBox = invAny.sheetsPerBox || productAny?.sheetsPerBox || 0;
    const piecesPerSheet = invAny.piecesPerSheet || productAny?.piecesPerSheet || 0;
    let piecePerBox = invAny.piecePerBox || productAny?.piecePerBox || null;
    
    // Calculate piecePerBox from prices if not available
    if (!piecePerBox || piecePerBox === 0) {
      const boxPrice = invAny.boxPrice || productAny?.boxPrice || 0;
      const piecePrice = invAny.piecePrice || productAny?.piecePrice || 0;
      if (boxPrice > 0 && piecePrice > 0) {
        piecePerBox = Math.round(boxPrice / piecePrice);
      }
    }
    
    const isMultiUnit = invAny.isMultiUnit || false;
    const boxQuantity = invAny.boxQuantity || 0;
    const pieceQuantity = invAny.pieceQuantity || 0;
    
    // Priority 2: Multi-Level (Box → Sheet → Piece)
    if (sheetsPerBox > 0 && piecesPerSheet > 0) {
      return 'Multi-Level';
    }
    
    // Priority 3: Dual-Level (Box → Piece)
    const hasPiecePerBox = piecePerBox !== null && piecePerBox > 0;
    const hasBothBoxAndPiece = boxQuantity > 0 && pieceQuantity >= 0;
    const isBoxToPieceStructure = isMultiUnit && boxQuantity > 0 && sheetsPerBox === 0 && piecesPerSheet === 0;
    
    if ((hasPiecePerBox || hasBothBoxAndPiece || isBoxToPieceStructure) && 
        piecesPerSheet === 0 && 
        sheetsPerBox === 0) {
      return 'Dual-Level';
    }
    
    // Priority 4: Single-Level (Pieces only)
    const hasNoConversion = sheetsPerBox === 0 && 
                            piecesPerSheet === 0 && 
                            (!piecePerBox || piecePerBox === 0) &&
                            boxQuantity === 0;
    
    if (hasNoConversion && pieceQuantity > 0) {
      return 'Single-Level';
    }
    
    // Default: If has boxes but no sheet conversion, treat as Dual-Level
    if (boxQuantity > 0 && sheetsPerBox === 0 && piecesPerSheet === 0) {
      return 'Dual-Level';
    }
    
    return 'Single-Level';
  }
}