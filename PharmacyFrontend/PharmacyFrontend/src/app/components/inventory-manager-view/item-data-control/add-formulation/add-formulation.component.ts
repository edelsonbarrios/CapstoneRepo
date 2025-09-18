import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-add-formulation',
  standalone: true,
  imports: [],
  templateUrl: './add-formulation.component.html',
  styleUrl: './add-formulation.component.scss'
})
export class AddFormulationComponent {
  @Input() addFormulation: Product = new Product;
}
