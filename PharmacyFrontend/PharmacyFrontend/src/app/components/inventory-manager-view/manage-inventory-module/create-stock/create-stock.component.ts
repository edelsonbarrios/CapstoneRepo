import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { PostService } from '../../../../services/PostService/post.service';
import { GetService } from '../../../../services/GetService/get.service';
import { Product } from '../../../../models/product-models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-stock',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-stock.component.html',
  styleUrl: './create-stock.component.scss'
})
export class CreateStockComponent implements OnInit{
  @Input() createStock: CreateInventory = new CreateInventory;
  @Output() closeForm = new EventEmitter<void>();
  products : Product[] = [];

  constructor(
    private postService : PostService,
    private getService : GetService
  ) { }

  ngOnInit(): void {
    this.getAllProducts();
  }

  createInventory() {
    if(!this.createStock) return;
    const requestPayload : CreateInventory = {
      productName : this.createStock.productName,
      batchNumber: this.createStock.batchNumber,
      manufacturingDate : new Date(this.createStock.manufacturingDate).toISOString(),
      expirationDate : new Date(this.createStock.expirationDate).toISOString(),
      quantityOnHand : this.createStock.quantityOnHand,
      reorderLevel : this.createStock.reorderLevel,
      costPrice : this.createStock.costPrice,
      sellingPrice : this.createStock.sellingPrice,
      unitCost : this.createStock.unitCost
    }
    this.postService.createNewInventoryAsync(requestPayload).subscribe({
      next: res => {
        alert(`Inventory successfully created.`);
        this.closeCreateInventoryForm();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateInventoryForm() {
    this.closeForm.emit();
  }

  getAllProducts() {
    this.getService.getAllProductsAsync().subscribe({
      next: res => {
        this.products = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}
