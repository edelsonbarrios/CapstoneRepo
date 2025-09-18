import { Component } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { AddGenericComponent } from '../add-generic/add-generic.component';
import { EditGenericComponent } from '../edit-generic/edit-generic.component';

@Component({
  selector: 'app-manage-generic-name',
  standalone: true,
  imports: [NgIf,AddGenericComponent,EditGenericComponent],
  templateUrl: './manage-generic-name.component.html',
  styleUrl: './manage-generic-name.component.scss'
})
export class ManageGenericNameComponent {
  addGeneric ?: Product | null;
  editGeneric ?: Product | null;

  openAddGenericForm() {
    this.addGeneric = new Product();
  }

  openEditGenericForm() {
    this.editGeneric = new Product();
  }
}
