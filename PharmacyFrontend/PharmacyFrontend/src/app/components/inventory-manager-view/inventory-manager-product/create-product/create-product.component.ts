import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { CreateProduct } from '../../../../models/product-models/createProduct';

@Component({
  selector: 'app-create-product',
  standalone: true,
  imports: [],
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.scss'
})
export class CreateProductComponent {
  @Input() newProduct: CreateProduct = new CreateProduct;
}
