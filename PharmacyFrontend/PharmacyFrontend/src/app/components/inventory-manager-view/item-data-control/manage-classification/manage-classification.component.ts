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
  paginatedClassifications: Classification[] = [];

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

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
        this.updatePagination();
      },
      error : err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.classifications.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedClassifications = this.classifications.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
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