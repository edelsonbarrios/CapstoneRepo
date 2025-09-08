import { Component, Input } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';

@Component({
  selector: 'app-create-stock',
  standalone: true,
  imports: [],
  templateUrl: './create-stock.component.html',
  styleUrl: './create-stock.component.scss'
})
export class CreateStockComponent {
  @Input() createStock: CreateInventory = new CreateInventory;
}
