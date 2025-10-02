import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-view-entry',
  standalone: true,
  imports: [],
  templateUrl: './view-entry.component.html',
  styleUrl: './view-entry.component.scss'
})
export class ViewEntryComponent {
@Input() viewEntry: Product = new Product();
}
