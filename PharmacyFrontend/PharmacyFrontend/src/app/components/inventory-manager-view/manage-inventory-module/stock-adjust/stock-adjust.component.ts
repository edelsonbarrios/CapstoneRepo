import { Component, Input } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';

@Component({
  selector: 'app-stock-adjust',
  standalone: true,
  imports: [],
  templateUrl: './stock-adjust.component.html',
  styleUrl: './stock-adjust.component.scss'
})
export class StockAdjustComponent {
  @Input() stockAdjust: CreateStockMovement = new CreateStockMovement;
}
