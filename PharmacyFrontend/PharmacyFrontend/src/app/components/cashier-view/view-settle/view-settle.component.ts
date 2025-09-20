import { Component, Input } from '@angular/core';
import { Product } from '../../../models/product-models/product';

@Component({
  selector: 'app-view-settle',
  standalone: true,
  imports: [],
  templateUrl: './view-settle.component.html',
  styleUrl: './view-settle.component.scss'
})
export class ViewSettleComponent {
  @Input() viewSettle: Product = new Product;
}
