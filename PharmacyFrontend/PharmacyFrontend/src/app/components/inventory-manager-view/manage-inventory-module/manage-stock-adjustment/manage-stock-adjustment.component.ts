import { Component, OnInit } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { StockAdjustComponent } from '../stock-adjust/stock-adjust.component';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';

@Component({
  selector: 'app-manage-stock-adjustment',
  standalone: true,
  imports: [StockAdjustComponent,
    NgIf, 
    CommonModule, 
    FormsModule,
    DatePipe
  ],
  templateUrl: './manage-stock-adjustment.component.html',
  styleUrl: './manage-stock-adjustment.component.scss'
})
export class ManageStockAdjustmentComponent implements OnInit{
  stockAdjust ?: Inventory | null;
  inventories: Inventory[] = [];

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

  openStockAdjustmentForm(inventory : Inventory) {
    this.stockAdjust = inventory
  }

  closeStockAdjustmentForm() {
    this.stockAdjust = null;
  }
}
