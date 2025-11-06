import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Product } from '../../../models/product-models/product';
import { NgIf, DatePipe } from '@angular/common';
import { ViewCashierProductsComponent } from '../view-cashier-products/view-cashier-products.component';
import { LogInToken } from '../../shared/token';

@Component({
  selector: 'app-cashier-header',
  standalone: true,
  imports: [RouterLink, NgIf, ViewCashierProductsComponent, DatePipe],
  templateUrl: './cashier-header.component.html',
  styleUrl: './cashier-header.component.scss',
})
export class CashierHeaderComponent implements OnInit, OnDestroy {
  viewCashierProduct?: Product | null;
  dateTime: Date = new Date();
  timer: any;

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Update date/time every second
    this.timer = setInterval(() => {
      this.dateTime = new Date();
    }, 1000);
  }

  ngOnDestroy(): void {
    // Clean up timer when component is destroyed
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  openViewCashierProductForm() {
    this.viewCashierProduct = new Product();
  }

  closeCashierProductForm() {
    this.viewCashierProduct = null;
  }

  logOut() {
    localStorage.removeItem(LogInToken);
    this.router.navigate(['/log-in'], { replaceUrl: true });
    this.router.navigateByUrl('/log-in');
  }
}
