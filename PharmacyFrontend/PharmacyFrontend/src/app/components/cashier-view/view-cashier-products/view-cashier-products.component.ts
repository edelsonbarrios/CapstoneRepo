import { Component,Input } from '@angular/core';
import { Product } from '../../../models/product-models/product';

@Component({
  selector: 'app-view-cashier-products',
  standalone: true,
  imports: [],
  templateUrl: './view-cashier-products.component.html',
  styleUrl: './view-cashier-products.component.scss'
})
export class ViewCashierProductsComponent {
  @Input() viewSettle: Product = new Product;
}
