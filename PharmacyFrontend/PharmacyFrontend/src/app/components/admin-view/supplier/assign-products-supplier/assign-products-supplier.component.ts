import { CurrencyPipe, NgFor } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { AssignProductSupplierRequest } from '../../../../models/supplier-models/assign-product-supplier';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-assign-products-supplier',
  standalone: true,
  imports: [CurrencyPipe, FormsModule, NgFor],
  templateUrl: './assign-products-supplier.component.html',
  styleUrl: './assign-products-supplier.component.scss'
})
export class AssignProductsSupplierComponent implements OnInit{
  @Input() supplier : Supplier = new Supplier();
  @Output() close = new EventEmitter<void>();
  products : Product[] = [];

  constructor(
    private getService : GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  toggleAll(event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    this.products.forEach(p => (p.selected = checked));
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

  assignProducts() {
    if(!this.supplier || this.products.length == 0) return;

    const selectedProducts = this.products.filter(p => p.selected);

    const requestPayload : AssignProductSupplierRequest = {
      supplierId : this.supplier.id,
      productsToBeAdded : selectedProducts
    };

    this.updateService.assignProductsToSupplierAsync(requestPayload).subscribe({
      next: res => {
        alert(`${res.message}`);
        this.close.emit();
      },
      error: err => {
        alert(`${err.error}`);
        console.log(`${err.error}`);
        console.log(`${err.error.message}`);
      }
    });
  }
}
