import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Discount } from '../../../../models/discount-models/discount';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-assign-inventory-discount',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-inventory-discount.component.html',
  styleUrl: './assign-inventory-discount.component.scss'
})
export class AssignInventoryDiscountComponent implements OnInit {
  @Input() discount: Discount = new Discount();
  @Output() inventoriesSelected = new EventEmitter<Inventory[]>();
  @Output() close = new EventEmitter<void>();

  inventories: Inventory[] = [];
  searchQuery: string = '';
  addItem: boolean = false;
  assignMessage : string = '';

  // Pagination
  paginatedInventories: Inventory[] = [];
  currentPage = 1;
  itemsPerPage = 8;
  totalPages = 0;

  constructor(
    private getService: GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.loadInventories();
  }

  closeAssignModal() {
    this.close.emit();
  }

  assignInventoryDiscounts() {
    if(this.discount == null) {
      console.log('Discount is empty');
      return;
    }
    const selectedInventories = this.inventories.filter(iv => iv.selected);

    if(selectedInventories.length === 0) {
      this.addItem = true;
      return;
    }

    this.updateService.assignInventoryDiscountsAsync(this.discount.id, selectedInventories).subscribe({
      next: res => {
        this.assignMessage = res.message;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeAssignMessage() {
    this.assignMessage = '';
    this.close.emit();
  }

  loadInventories(): void {
    this.getService.getAllAvailableInventoriesAsync().subscribe({
      next: (inventories) => {
        this.inventories = inventories.map(inv => ({
          ...inv,
          selected: false
        }));
        this.updatePagination();
      },
      error: (err) => {
        console.error('Error loading inventories:', err);
      }
    });
  }

  // Filtered inventories based on search query
  get filteredInventories(): Inventory[] {
    if (!this.searchQuery.trim()) {
      return this.inventories;
    }
    const query = this.searchQuery.toLowerCase();
    return this.inventories.filter(inventory =>
      inventory.productName?.toLowerCase().includes(query) ||
      inventory.categoryName?.toLowerCase().includes(query) ||
      inventory.brandName?.toLowerCase().includes(query) ||
      inventory.batchNumber?.toLowerCase().includes(query)
    );
  }

  get selectedCount(): number {
    return this.inventories.filter(inv => inv.selected).length;
  }

  updatePagination() {
    const items = this.filteredInventories;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;

    // Reset to page 1 if current page exceeds total pages
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedInventories = items.slice(startIndex, endIndex);
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
    this.paginatedInventories.forEach(inv => {
      const originalIndex = this.inventories.findIndex(i => i.inventoryID === inv.inventoryID);
      if (originalIndex !== -1) {
        this.inventories[originalIndex].selected = checked;
      }
    });
  }

  formatDate(date: Date | string): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getStockStatus(inventory: Inventory): string {
    if (inventory.isExpired) return 'Expired';
    if (inventory.isNearExpiry) return 'Near Expiry';
    if (inventory.isCriticalStock) return 'Low Stock';
    return 'Good';
  }

  getStockStatusClass(inventory: Inventory): string {
    if (inventory.isExpired) return 'status-expired';
    if (inventory.isNearExpiry) return 'status-near-expiry';
    if (inventory.isCriticalStock) return 'status-low';
    return 'status-good';
  }

  addSelected() {
    const selected = this.inventories.filter(inv => inv.selected);
    if (selected.length === 0) {
      this.addItem = true;
      return;
    }
    this.inventoriesSelected.emit(selected);
  }

  closeAddItemMessage() {
    this.addItem = false;
  }

  cancel() {
    this.close.emit();
  }
}
