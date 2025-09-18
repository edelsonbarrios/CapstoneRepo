import { Component } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { AddTypeComponent } from '../add-type/add-type.component';
import { EditTypeComponent } from '../edit-type/edit-type.component';

@Component({
  selector: 'app-manage-type',
  standalone: true,
  imports: [NgIf,AddTypeComponent,EditTypeComponent],
  templateUrl: './manage-type.component.html',
  styleUrl: './manage-type.component.scss'
})
export class ManageTypeComponent {
  addType ?: Product | null;
  editType ?: Product | null;

  openAddTypeForm() {
    this.addType = new Product();
  }

  openEditTypeForm() {
    this.editType = new Product();
  }
}
