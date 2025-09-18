import { Component } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { AddClassificationComponent } from '../add-classification/add-classification.component';
import { EditClassificationComponent } from '../edit-classification/edit-classification.component';

@Component({
  selector: 'app-manage-classification',
  standalone: true,
  imports: [NgIf,AddClassificationComponent,EditClassificationComponent],
  templateUrl: './manage-classification.component.html',
  styleUrl: './manage-classification.component.scss'
})
export class ManageClassificationComponent {
  addClassification ?: Product | null;
  editClassification ?: Product | null;

  openAddClassificationForm() {
    this.addClassification = new Product();
  }

  openEditClassificationForm() {
    this.editClassification = new Product();
  }
}
