import { Component, OnInit } from '@angular/core';
import { NgFor, NgIf, CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../../services/UpdateService/update.service';
import { ViewEntryComponent } from '../../../../inventory-manager-view/manage-inventory-module/view-entry/view-entry.component';
import { Inventory } from '../../../../../models/inventory-module-models/inventory';
import { GetService } from '../../../../../services/GetService/get.service';
import { CreateStockComponent } from '../../../../inventory-manager-view/manage-inventory-module/create-stock/create-stock.component';
import { CreateInventory } from '../../../../../models/inventory-module-models/createInventory';


@Component({
  selector: 'app-manage-stock-entry',
  standalone: true,
  imports: [CreateStockComponent, NgIf, FormsModule, CommonModule, DatePipe, ViewEntryComponent, NgFor],
  templateUrl: './admin-manage-stock-entry.component.html',
  styleUrl: './admin-manage-stock-entry.component.scss'
})
export class ManageStockEntryComponent implements OnInit {
  createStock?: CreateInventory | null;
  viewEntry?: Inventory | null;
  inventories: Inventory[] = [];
  paginatedInventories: Inventory[] = [];
  searchQuery: string = '';
  showDeleteConfirmation: boolean = false;
  inventoryToDelete?: Inventory | null;
  
  // New properties for filtering
  selectedFilter: 'all' | 'expired' | 'nearExpiry' | 'healthy' = 'all';

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllInventories();
  }

  // Calculate days until expiration
  getDaysUntilExpiry(expirationDate: string | Date): number {
    const expiry = new Date(expirationDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    expiry.setHours(0, 0, 0, 0);
    const diffTime = expiry.getTime() - today.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  }

  // Get expiry status: 'critical' (red), 'warning' (yellow), 'healthy' (green)
  getExpiryStatus(expirationDate: string | Date): 'critical' | 'warning' | 'healthy' {
    const days = this.getDaysUntilExpiry(expirationDate);
    if (days <= 180) return 'critical'; // 6 months or less
    if (days <= 365) return 'warning';  // 7-12 months
    return 'healthy'; // more than 1 year
  }

  // Get CSS class for expiry status
  getExpiryClass(expirationDate: string | Date): string {
    const days = this.getDaysUntilExpiry(expirationDate);

    if (days < 0) return 'expired';          // red
    if (days <= 365) return 'nearExpiry';    // yellow
    return 'healthy';                        // green
  }

  // Get human-readable expiry label
  getExpiryLabel(expirationDate: string | Date): string {
    const days = this.getDaysUntilExpiry(expirationDate);
    
    if (days < 0) {
      return `Expired ${Math.abs(days)} days ago`;
    } else if (days === 0) {
      return 'Expires today';
    } else if (days === 1) {
      return 'Expires tomorrow';
    } else if (days <= 30) {
      return `${days} days left`;
    } else if (days <= 365) {
      const months = Math.floor(days / 30);
      return `${months} month${months > 1 ? 's' : ''} left`;
    } else {
      const years = Math.floor(days / 365);
      const remainingMonths = Math.floor((days % 365) / 30);
      if (remainingMonths === 0) {
        return `${years} year${years > 1 ? 's' : ''} left`;
      }
      return `${years}y ${remainingMonths}m left`;
    }
  }

  // Get count for each filter category
  getFilterCount(filter: 'all' | 'expired' | 'nearExpiry' | 'healthy'): number {
    const filtered = this.inventories.filter(inv => {
      const days = this.getDaysUntilExpiry(inv.expirationDate);
      
      switch (filter) {
        case 'all':
          return true;
        case 'expired':
          return days < 0;
        case 'nearExpiry':
          return days >= 0 && days <= 365; // Within 1 year
        case 'healthy':
          return days > 365; // More than 1 year
        default:
          return true;
      }
    });
    return filtered.length;
  }

  // Handle filter change
  onFilterChange(filter: 'all' | 'expired' | 'nearExpiry' | 'healthy') {
    this.selectedFilter = filter;
    this.currentPage = 1;
    this.updatePagination();
  }

  // Updated filtered inventories to include expiry filter
  get filteredInventories(): Inventory[] {
    let filtered = this.inventories;

    // Apply search filter
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      filtered = filtered.filter(inv =>
        inv.batchNumber.toLowerCase().includes(query) ||
        inv.productName.toLowerCase().includes(query)
      );
    }

    // Apply expiry filter
    filtered = filtered.filter(inv => {
      const days = this.getDaysUntilExpiry(inv.expirationDate);
      
      switch (this.selectedFilter) {
        case 'all':
          return true;
        case 'expired':
          return days < 0;
        case 'nearExpiry':
          return days >= 0 && days <= 365;
        case 'healthy':
          return days > 365;
        default:
          return true;
      }
    });

    return filtered;
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  getAllInventories() {
    this.getService.getAllInventoriesAsync().subscribe({
      next: res => {
        this.inventories = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    const items = this.filteredInventories;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedInventories = items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openCreateStockForm() {
    this.createStock = new CreateInventory();
  }

  openViewEntryForm(inv: Inventory) {
    this.viewEntry = inv;
  }

  closeCreateStockForm() {
    this.createStock = null;
    this.getAllInventories();
  }

  closeViewStockForm() {
    this.viewEntry = null;
  }

  openDeleteConfirmation(inventory: Inventory) {
    this.inventoryToDelete = inventory;
    this.showDeleteConfirmation = true;
  }

  closeDeleteConfirmation() {
    this.showDeleteConfirmation = false;
    this.inventoryToDelete = null;
  }

  confirmDelete() {
    if (!this.inventoryToDelete) return;

    this.updateService.archiveInventoryByIdAsync(this.inventoryToDelete.inventoryID).subscribe({
      next: res => {
        console.log(`${res.message}`);
        this.showDeleteConfirmation = false;
        this.getAllInventories();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}