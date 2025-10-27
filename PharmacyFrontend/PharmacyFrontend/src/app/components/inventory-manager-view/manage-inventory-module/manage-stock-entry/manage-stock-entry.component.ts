import { Component, OnInit } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { NgFor, NgIf, CommonModule, DatePipe } from '@angular/common';
import { CreateStockComponent } from '../create-stock/create-stock.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { FormsModule } from '@angular/forms';
import { ViewEntryComponent } from '../view-entry/view-entry.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-manage-stock-entry',
  standalone: true,
  imports: [CreateStockComponent, NgIf, FormsModule, CommonModule, DatePipe, ViewEntryComponent, NgFor],
  templateUrl: './manage-stock-entry.component.html',
  styleUrl: './manage-stock-entry.component.scss'
})
export class ManageStockEntryComponent implements OnInit {
  createStock?: CreateInventory | null;
  viewEntry?: Inventory | null;
  inventories: Inventory[] = [];
  paginatedInventories: Inventory[] = [];
  searchQuery: string = '';
  showDeleteConfirmation: boolean = false;
  inventoryToDelete ?: Inventory | null;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllInventories();
  }

  get filteredInventories(): Inventory[] {
    if (!this.searchQuery.trim()) {
      return this.inventories;
    }
    const query = this.searchQuery.toLowerCase();
    return this.inventories.filter(inv =>
      inv.batchNumber.toLowerCase().includes(query) ||
      inv.productName.toLowerCase().includes(query)
    );
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
