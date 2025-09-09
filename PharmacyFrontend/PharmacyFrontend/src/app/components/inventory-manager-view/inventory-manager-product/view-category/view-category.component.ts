import { Component, Input } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Category } from '../../../../models/product-models/category';

@Component({
  selector: 'app-view-category',
  standalone: true,
  imports: [],
  templateUrl: './view-category.component.html',
  styleUrl: './view-category.component.scss'
})
export class ViewCategoryComponent {
  @Input() viewCategory: Category = new Category; 
}
