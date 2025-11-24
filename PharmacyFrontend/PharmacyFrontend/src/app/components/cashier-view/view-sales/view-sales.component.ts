import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Product } from '../../../models/product-models/product';
import { CurrencyPipe, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { SalesTransaction } from '../../../models/sales-transaction-models/sales-transaction';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { SalesItem } from '../../../models/sales-transaction-models/sales-item';
import { Discount } from '../../../models/discount-models/discount';

@Component({
  selector: 'app-view-sales',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, DecimalPipe],
  templateUrl: './view-sales.component.html',
  styleUrl: './view-sales.component.scss'
})
export class ViewSalesComponent implements OnInit, OnChanges {
  @Input() viewSales: SalesTransaction = new SalesTransaction();
  @Output() closeForm = new EventEmitter<void>();
  paginatedItems: SalesItem[] = [];
  cashier: Employee = new Employee();
  appliedDiscount: Discount | null = null;
  appliedVat: any = null;
  
  // Cache for products to avoid multiple API calls
  private productCache: Map<number, Product> = new Map();
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  constructor(
    private getService: GetService
  ) {}

  ngOnInit(): void {
    this.updatePagination();
    this.getCashier();
    this.preloadProducts(); // Load all products for items
    this.getAppliedDiscount(); // Load discount details
    this.getAppliedVat(); // Load VAT details
  }
 
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items']) {
      this.currentPage = 1;
      this.updatePagination();
      this.preloadProducts();
    }
    if (changes['viewSales']) {
      this.getAppliedDiscount();
      this.getAppliedVat();
    }
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.viewSales.items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.viewSales.items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  /**
   * Preload all products for the sales items to avoid multiple API calls
   */
  preloadProducts() {
    const uniqueProductIds = [...new Set(this.viewSales.items.map(item => item.productId))];
    
    uniqueProductIds.forEach(productId => {
      if (!this.productCache.has(productId)) {
        this.getService.getProductByIdAsync(productId).subscribe({
          next: res => {
            this.productCache.set(productId, res);
          },
          error: err => {
            console.log(`Error loading product ${productId}: ${err.error}`);
          }
        });
      } 
    });
  }

  /**
   * Get discount details for the transaction
   */
  getAppliedDiscount() {
    if (!this.viewSales.discountId || this.viewSales.discountId === 0) {
      this.appliedDiscount = null;
      return;
    }

    this.getService.getAllDiscountAsync().subscribe({
      next: res => {
        this.appliedDiscount = res.find(d => d.id === this.viewSales.discountId) || null;
      },
      error: err => {
        console.log(`Error loading discount: ${err.error}`);
        this.appliedDiscount = null;
      }
    });
  }

  /**
   * Get discount display text with type and amount
   */
  getDiscountDisplayText(): string {
    if (!this.appliedDiscount) return 'No Discount';

    if (this.appliedDiscount.discountType === 'Percentage') {
      return `${this.appliedDiscount.discountName} (${this.appliedDiscount.discountAmount}%)`;
    } else if (this.appliedDiscount.discountType === 'Fixed') {
      return `${this.appliedDiscount.discountName} (₱${
        this.appliedDiscount.discountAmount?.toFixed(2) || '0.00'
      })`;
    }

    return this.appliedDiscount.discountName;
  }

  /**
   * Get VAT details for the transaction
   */
  getAppliedVat() {
    if (!this.viewSales.vatId || this.viewSales.vatId === 0) {
      this.appliedVat = null;
      return;
    }

    this.getService.getAllVatAsync().subscribe({
      next: res => {
        this.appliedVat = res.find(v => v.vatid === this.viewSales.vatId) || null;
      },
      error: err => {
        console.log(`Error loading VAT: ${err.error}`);
        this.appliedVat = null;
      }
    });
  }

  /**
   * Get VAT display text with percentage
   */
  getVatDisplayText(): string {
    if (!this.appliedVat) return 'No VAT';
    return `${this.appliedVat.vatName} (${this.appliedVat.vatRate}%)`;
  }

  /**
   * Convert stored piece quantity back to the original unit for display
   * Since backend stores everything in pieces, we need to reverse the conversion
   */
  getUnitQuantity(item: SalesItem): number {
    const product = this.productCache.get(item.productId);
    
    if (!product) {
      // If product not loaded yet, return the stored quantity as fallback
      return item.quantity;
    }

    const unitType = item.unitType.toLowerCase();

    // Based on your backend logic in ConvertToPieces method
    if (unitType === 'box') {
      // Multi-Level: Box → Sheets → Pieces
      if (product.sheetsPerBox && product.piecesPerSheet) {
        const piecesPerBox = product.sheetsPerBox * product.piecesPerSheet;
        return Math.round(item.quantity / piecesPerBox);
      }
      // Dual-Level: Box → Pieces
      else if (product.piecePerBox) {
        return Math.round(item.quantity / product.piecePerBox);
      }
    } 
    else if (unitType === 'sheet') {
      // Multi-Level only: Sheet → Pieces
      if (product.piecesPerSheet) {
        return Math.round(item.quantity / product.piecesPerSheet);
      }
    }
    
    // For 'piece' or if no conversion ratios exist
    return item.quantity;
  }

  /**
   * Calculate the correct subtotal using the display quantity
   */
  getDisplaySubtotal(item: SalesItem): number {
    const displayQty = this.getUnitQuantity(item);
    return displayQty * item.unitPrice;
  }

  closeViewSales() {
    this.closeForm.emit();
  }

  getCashier() {
    this.getService.GetEmployeeById(this.viewSales.employeeID).subscribe({
      next: res => {
        this.cashier = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}