import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Product } from '../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { ViewCashierProductsComponent } from '../view-cashier-products/view-cashier-products.component';
import { LogInToken } from '../../shared/token';

@Component({
  selector: 'app-cashier-header',
  standalone: true,
  imports: [RouterLink,NgIf,ViewCashierProductsComponent],
  templateUrl: './cashier-header.component.html',
  styleUrl: './cashier-header.component.scss'
})
export class CashierHeaderComponent {
  viewCashierProduct ?: Product | null;

  constructor(
    private router : Router
  ) {}
  

  openViewCashierProductForm() {
    this.viewCashierProduct = new Product();
  }

  closeCashierProductForm() {
    this.viewCashierProduct = null;
  }

  logOut() {
    localStorage.removeItem(LogInToken);
    this.router.navigateByUrl('/log-in');
  }
}
