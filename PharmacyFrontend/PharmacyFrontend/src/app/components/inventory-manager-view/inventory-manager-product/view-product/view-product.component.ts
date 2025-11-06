import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-view-product',
  standalone: true,
  imports: [FormsModule, NgIf, CommonModule],
  templateUrl: './view-product.component.html',
  styleUrl: './view-product.component.scss'
})
export class ViewProductComponent  implements OnInit{
  @Input() viewProduct: Product = new Product();
  @Output() closeForm = new EventEmitter<void>();

  ngOnInit(): void {
    console.log(this.viewProduct);
  }


  closeViewProductForm() {
    this.closeForm.emit();
  }
}
