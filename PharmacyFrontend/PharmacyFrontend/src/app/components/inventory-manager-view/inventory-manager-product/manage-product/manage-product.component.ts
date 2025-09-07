import { Component } from '@angular/core';
import { CreateProductComponent } from '../create-product/create-product.component';
import { Product } from '../../../../models/product';
import { NgIf } from '@angular/common';
import { ViewProductComponent } from '../view-product/view-product.component';

@Component({
  selector: 'app-manage-product',
  standalone: true,
  imports: [CreateProductComponent,NgIf,ViewProductComponent],
  templateUrl: './manage-product.component.html',
  styleUrl: './manage-product.component.scss'
})
export class ManageProductComponent {
  newProduct ?: Product | null;
  viewProduct ?: Product | null;

  openCreateProductForm(){
    this.newProduct= new Product();
  }

  openViewProductForm() {
    this.viewProduct= new Product();
  }
}
