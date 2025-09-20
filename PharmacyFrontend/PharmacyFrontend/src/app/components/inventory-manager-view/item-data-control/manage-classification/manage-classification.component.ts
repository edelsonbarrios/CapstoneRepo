import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddClassificationComponent } from '../add-classification/add-classification.component';
import { EditClassificationComponent } from '../edit-classification/edit-classification.component';
import { Classification } from '../../../../models/classification-models/classification';
import { GetService } from '../../../../services/GetService/get.service';

@Component({
  selector: 'app-manage-classification',
  standalone: true,
  imports: [NgIf,AddClassificationComponent,EditClassificationComponent, NgFor],
  templateUrl: './manage-classification.component.html',
  styleUrl: './manage-classification.component.scss'
})
export class ManageClassificationComponent implements OnInit{
  addClassification ?: Classification | null;
  editClassification ?: Classification | null;
  classifications : Classification[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllClassifications();
  }

  getAllClassifications() {
    this.getService.getAllClassificationAsync().subscribe({
      next: res => {
        this.classifications = res;
      },
      error : err => {
        console.log(`${err.error}`);
      }
    });
  }

  openAddClassificationForm() {
    this.addClassification = new Classification();
  }

  closeAddClassificationForm() {
    this.addClassification = null;
    this.getAllClassifications();
  }

  openEditClassificationForm(classification : Classification) {
    this.editClassification = classification;
  }

  closeEditClassificationForm() {
    this.editClassification = null;
  }
}
