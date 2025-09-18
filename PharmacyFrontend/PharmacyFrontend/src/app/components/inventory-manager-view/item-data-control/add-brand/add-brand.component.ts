import { Component,Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [],
  templateUrl: './add-brand.component.html',
  styleUrl: './add-brand.component.scss'
})
export class AddBrandComponent {
  @Input() addBrand: Product = new Product;
}
