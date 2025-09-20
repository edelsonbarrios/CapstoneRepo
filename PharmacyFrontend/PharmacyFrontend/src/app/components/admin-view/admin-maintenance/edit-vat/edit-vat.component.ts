import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-vat',
  standalone: true,
  imports: [],
  templateUrl: './edit-vat.component.html',
  styleUrl: './edit-vat.component.scss'
})
export class EditVatComponent {
  @Input()  editVat: Product = new Product;
}
