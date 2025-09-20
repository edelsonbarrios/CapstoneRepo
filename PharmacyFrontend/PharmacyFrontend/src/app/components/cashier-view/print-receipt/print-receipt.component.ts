import { Component, Input } from '@angular/core';
import { Product } from '../../../models/product-models/product';

@Component({
  selector: 'app-print-receipt',
  standalone: true,
  imports: [],
  templateUrl: './print-receipt.component.html',
  styleUrl: './print-receipt.component.scss'
})
export class PrintReceiptComponent {
 @Input() printReceipt: Product = new Product;
}
