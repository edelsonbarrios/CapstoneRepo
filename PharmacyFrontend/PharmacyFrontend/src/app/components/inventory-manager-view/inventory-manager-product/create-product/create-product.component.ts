import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product';

@Component({
  selector: 'app-create-product',
  standalone: true,
  imports: [],
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.scss'
})
export class CreateProductComponent {
  @Input() newProduct: Product = new Product;
}
