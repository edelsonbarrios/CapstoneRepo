import { Component, Output, EventEmitter, OnInit, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, NgFor } from '@angular/common';
import { GetService } from '../../../../services/GetService/get.service';

@Component({
  selector: 'app-purchase-order-products',
  standalone: true,
  imports: [FormsModule, CurrencyPipe, NgFor],
  templateUrl: './purchase-order-products.component.html',
  styleUrl: './purchase-order-products.component.scss'
})
export class PurchaseOrderProductsComponent implements OnInit{
  @Input() supplierProudcts : Product[] = [];
  @Output() productsSelected = new EventEmitter<Product[]>();
  @Output() close = new EventEmitter<void>();

  products: Product[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllProducts();
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

  toggleAll(event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    this.supplierProudcts.forEach(p => (p.selected = checked));
  }

  addSelected() {
    const selected = this.supplierProudcts.filter(p => p.selected);
    console.log(selected);
    this.productsSelected.emit(selected);
    this.close.emit(); // auto-close after adding
  }

  cancel() {
    this.close.emit();
  }
}
