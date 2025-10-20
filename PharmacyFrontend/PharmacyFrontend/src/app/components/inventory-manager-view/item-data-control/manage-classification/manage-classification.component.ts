import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddClassificationComponent } from '../add-classification/add-classification.component';
import { EditClassificationComponent } from '../edit-classification/edit-classification.component';
import { Classification } from '../../../../models/classification-models/classification';
import { GetService } from '../../../../services/GetService/get.service';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-classification',
  standalone: true,
  imports: [
    NgIf,
    AddClassificationComponent,
    EditClassificationComponent, 
    NgFor,
    ArchiveConfirmationComponent,
    FormsModule
  ],
  templateUrl: './manage-classification.component.html',
  styleUrl: './manage-classification.component.scss'
})
export class ManageClassificationComponent implements OnInit{
  addClassification ?: Classification | null;
  editClassification ?: Classification | null;
  classifications : Classification[] = [];
  filteredClassifications: Classification[] = [];
  paginatedClassifications: Classification[] = [];
  showArchiveForm: number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  // Search property
  searchQuery: string = '';

  constructor(
    private getService : GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllClassifications();
  }

  getAllClassifications() {
    this.getService.getAllClassificationAsync().subscribe({
      next: res => {
        this.classifications = res;
        this.filteredClassifications = [...this.classifications];
        this.updatePagination();
      },
      error : err => {
        console.log(`${err.error}`);
      }
    });
  }

  onSearchChange(): void {
    const query = this.searchQuery.trim().toLowerCase();

    if (query === '') {
      this.filteredClassifications = [...this.classifications];
    } else {
      this.filteredClassifications = this.classifications.filter((c) =>
        c.classificationName.toLowerCase().includes(query)
      );
    }

    this.currentPage = 1;
    this.updatePagination();
  }

  openArchiveConfirmationForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveConfirmationForm() {
    this.showArchiveForm = 0;
  }

  archiveClassification() {
    this.updateService.archiveClassificationByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllClassifications();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredClassifications.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedClassifications = this.filteredClassifications.slice(startIndex, endIndex);
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