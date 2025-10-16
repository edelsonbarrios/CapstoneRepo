import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { CurrencyPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-view-inventory',
  standalone: true,
  imports: [NgIf],
  templateUrl: './view-inventory.component.html',
  styleUrl: './view-inventory.component.scss'
})
export class ViewInventoryComponent {
  @Input() viewInventory: any;
  @Output() closeForm = new EventEmitter<void>();

  closeViewInventoryForm() {
    this.closeForm.emit();
  }
}
