import { Component,Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-generic',
  standalone: true,
  imports: [],
  templateUrl: './edit-generic.component.html',
  styleUrl: './edit-generic.component.scss'
})
export class EditGenericComponent {
  @Input() editBrand: Product = new Product;
}
