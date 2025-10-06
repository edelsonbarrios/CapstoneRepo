import { Component, Input } from '@angular/core';
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';

@Component({
  selector: 'app-view-adjustment',
  standalone: true,
  imports: [],
  templateUrl: './view-adjustment.component.html',
  styleUrl: './view-adjustment.component.scss'
})
export class ViewAdjustmentComponent {
 @Input() viewAdjust: StockMovement = new StockMovement();
}
