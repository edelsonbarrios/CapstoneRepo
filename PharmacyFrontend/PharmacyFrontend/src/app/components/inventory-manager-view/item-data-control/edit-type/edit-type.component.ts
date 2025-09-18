import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-type',
  standalone: true,
  imports: [],
  templateUrl: './edit-type.component.html',
  styleUrl: './edit-type.component.scss'
})
export class EditTypeComponent {
  @Input() editType: Product = new Product;
}
