import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-edit-formulation',
  standalone: true,
  imports: [],
  templateUrl: './edit-formulation.component.html',
  styleUrl: './edit-formulation.component.scss'
})
export class EditFormulationComponent {
  @Input() editFormulation: Product = new Product;
}
