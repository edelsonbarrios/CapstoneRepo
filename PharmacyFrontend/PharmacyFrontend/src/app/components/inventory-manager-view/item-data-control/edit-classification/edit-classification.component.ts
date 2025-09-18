import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-classification',
  standalone: true,
  imports: [],
  templateUrl: './edit-classification.component.html',
  styleUrl: './edit-classification.component.scss'
})
export class EditClassificationComponent {
  @Input() editClassification: Product = new Product;
}
