import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product';

@Component({
  selector: 'app-view-product',
  standalone: true,
  imports: [],
  templateUrl: './view-product.component.html',
  styleUrl: './view-product.component.scss'
})
export class ViewProductComponent {
  @Input() viewProduct: Product = new Product;  
}
