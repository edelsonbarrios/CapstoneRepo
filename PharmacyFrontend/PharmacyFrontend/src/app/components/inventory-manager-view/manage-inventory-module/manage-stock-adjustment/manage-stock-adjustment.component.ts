import { Component } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { StockAdjustComponent } from '../stock-adjust/stock-adjust.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-manage-stock-adjustment',
  standalone: true,
  imports: [StockAdjustComponent,NgIf],
  templateUrl: './manage-stock-adjustment.component.html',
  styleUrl: './manage-stock-adjustment.component.scss'
})
export class ManageStockAdjustmentComponent {
  stockAdjust ?: CreateStockMovement | null;

  openStockAdjustmentForm() {
    this.stockAdjust = new CreateStockMovement;
  }
}
