import { Component, Input } from '@angular/core';
import { Product } from '../../../models/product-models/product';

@Component({
  selector: 'app-view-sales',
  standalone: true,
  imports: [],
  templateUrl: './view-sales.component.html',
  styleUrl: './view-sales.component.scss'
})
export class ViewSalesComponent {
@Input() viewSales: Product = new Product;
}
