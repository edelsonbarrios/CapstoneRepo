import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Product } from '../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { ViewCashierProductsComponent } from '../view-cashier-products/view-cashier-products.component';

@Component({
  selector: 'app-cashier-header',
  standalone: true,
  imports: [RouterLink,NgIf,ViewCashierProductsComponent],
  templateUrl: './cashier-header.component.html',
  styleUrl: './cashier-header.component.scss'
})
export class CashierHeaderComponent {
  viewCashierProduct ?: Product | null;

  openViewCashierProductForm() {
    this.viewCashierProduct = new Product();
  }
}
