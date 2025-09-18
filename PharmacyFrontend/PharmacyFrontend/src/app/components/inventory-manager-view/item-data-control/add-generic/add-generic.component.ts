import { Component,Input} from '@angular/core';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-add-generic',
  standalone: true,
  imports: [],
  templateUrl: './add-generic.component.html',
  styleUrl: './add-generic.component.scss'
})
export class AddGenericComponent {
  @Input() addGeneric: Product = new Product;
}
