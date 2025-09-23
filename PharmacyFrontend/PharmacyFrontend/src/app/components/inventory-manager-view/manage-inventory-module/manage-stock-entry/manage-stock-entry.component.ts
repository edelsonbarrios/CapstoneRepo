import { Component, OnInit } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { NgFor, NgIf, CommonModule } from '@angular/common';
import { CreateStockComponent } from '../create-stock/create-stock.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-stock-entry',
  standalone: true,
  imports: [CreateStockComponent, NgIf, FormsModule, CommonModule],
  templateUrl: './manage-stock-entry.component.html',
  styleUrl: './manage-stock-entry.component.scss'
})
export class ManageStockEntryComponent implements OnInit{
  createStock ?: CreateInventory | null;
  inventories : Inventory[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllInventories();
  }

  getAllInventories() {
    this.getService.getAllInventoriesAsync().subscribe({
      next: res => {
        this.inventories = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  openCreateStockForm() {
    this.createStock = new CreateInventory;
  }
}
