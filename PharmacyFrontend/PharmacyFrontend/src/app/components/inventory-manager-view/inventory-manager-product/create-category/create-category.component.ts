import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { CreateCategory } from '../../../../models/product-models/createCategory';

@Component({
  selector: 'app-create-category',
  standalone: true,
  imports: [],
  templateUrl: './create-category.component.html',
  styleUrl: './create-category.component.scss'
})
export class CreateCategoryComponent {
  @Input() newProduct: CreateCategory = new CreateCategory;
}
