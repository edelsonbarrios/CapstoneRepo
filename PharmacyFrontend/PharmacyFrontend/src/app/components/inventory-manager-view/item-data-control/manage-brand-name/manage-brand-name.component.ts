import { Component } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { AddBrandComponent } from '../add-brand/add-brand.component';
import { EditBrandComponent } from '../edit-brand/edit-brand.component';

@Component({
  selector: 'app-manage-brand-name',
  standalone: true,
  imports: [NgIf,AddBrandComponent,EditBrandComponent],
  templateUrl: './manage-brand-name.component.html',
  styleUrl: './manage-brand-name.component.scss'
})
export class ManageBrandNameComponent {
  addBrand ?: Product | null;
  editBrand ?: Product | null;

  openAddBrandForm() {
    this.addBrand = new Product();
  }

  openEditBrandForm() {
    this.editBrand = new Product();
  }
}
