import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Inventory } from '../../../../models/inventory-module-models/inventory';

@Component({
  selector: 'app-view-entry',
  standalone: true,
  imports: [],
  templateUrl: './view-entry.component.html',
  styleUrl: './view-entry.component.scss'
})
export class ViewEntryComponent {
  @Input() viewEntry: Inventory = new Inventory();
  @Output() closeForm = new EventEmitter<void>();

  closeViewStockEntry() {
    this.closeForm.emit();
  }
}
