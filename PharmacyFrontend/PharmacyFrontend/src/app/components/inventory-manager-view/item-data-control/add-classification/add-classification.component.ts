import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-add-classification',
  standalone: true,
  imports: [],
  templateUrl: './add-classification.component.html',
  styleUrl: './add-classification.component.scss'
})
export class AddClassificationComponent {
  @Input() addClassification: Product = new Product;
}
