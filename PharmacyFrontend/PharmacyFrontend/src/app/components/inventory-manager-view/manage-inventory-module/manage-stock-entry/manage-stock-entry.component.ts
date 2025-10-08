import { Component, OnInit } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { NgFor, NgIf, CommonModule, DatePipe } from '@angular/common';
import { CreateStockComponent } from '../create-stock/create-stock.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { FormsModule } from '@angular/forms';
import { Product } from '../../../../models/product-models/product';
import { ViewEntryComponent } from '../view-entry/view-entry.component';

@Component({
  selector: 'app-manage-stock-entry',
  standalone: true,
  imports: [CreateStockComponent, NgIf, FormsModule, CommonModule, DatePipe, ViewEntryComponent],
  templateUrl: './manage-stock-entry.component.html',
  styleUrl: './manage-stock-entry.component.scss'
})
export class ManageStockEntryComponent implements OnInit{
  createStock ?: CreateInventory | null;
  viewEntry ?: Inventory | null;
  inventories : Inventory[] = [];
  paginatedInventories: Inventory[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllInventories();
  }

  getAllInventories() {
    this.getService.getAllAvailableInventoriesAsync().subscribe({
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
    this.totalPages = Math.ceil(this.inventories.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedInventories = this.inventories.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openCreateStockForm() {
    this.createStock = new CreateInventory;
  }

  openViewEntryForm(inv : Inventory) {
    this.viewEntry = inv;
  }

  closeCreateStockForm() {
    this.createStock = null;
  }

  closeViewStockForm() {
    this.viewEntry = null;
  }
}