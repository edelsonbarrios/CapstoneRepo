import { Component } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-manage-stock-entry',
  standalone: true,
  imports: [],
  templateUrl: './manage-stock-entry.component.html',
  styleUrl: './manage-stock-entry.component.scss'
})
export class ManageStockEntryComponent {
  createStock ?: CreateInventory | null;

  openCreateCategoryForm() {
    this.createStock = new CreateInventory;
  }
}
