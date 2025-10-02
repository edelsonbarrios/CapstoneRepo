import { Component, OnInit } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { StockAdjustComponent } from '../stock-adjust/stock-adjust.component';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';

@Component({
  selector: 'app-manage-stock-adjustment',
  standalone: true,
  imports: [StockAdjustComponent,
    NgIf, 
    CommonModule, 
    FormsModule,
  ],
  templateUrl: './manage-stock-adjustment.component.html',
  styleUrl: './manage-stock-adjustment.component.scss'
})
export class ManageStockAdjustmentComponent implements OnInit{
  stockAdjust ?: StockMovement | null;
  stockMovements: StockMovement[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllStockMovements();
  }

  getAllStockMovements() {
    this.getService.getAllStockMovementsAsync().subscribe({
      next: res => {
        this.stockMovements = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  openStockAdjustmentForm(stockMovement : StockMovement) {
    this.stockAdjust = stockMovement;
  }

  closeStockAdjustmentForm() {
    this.stockAdjust = null;
  }
}
