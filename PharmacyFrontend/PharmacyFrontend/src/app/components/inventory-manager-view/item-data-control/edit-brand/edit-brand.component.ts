import { Component,Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-brand',
  standalone: true,
  imports: [],
  templateUrl: './edit-brand.component.html',
  styleUrl: './edit-brand.component.scss'
})
export class EditBrandComponent {
  @Input() editBrand: Product = new Product;
}
