import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { AddFormulationComponent } from '../add-formulation/add-formulation.component';
import { EditFormulationComponent } from '../edit-formulation/edit-formulation.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-formulation',
  standalone: true,
  imports: [
    NgIf,
    AddFormulationComponent,
    EditFormulationComponent, 
    NgFor,
    ArchiveConfirmationComponent,
    FormsModule
  ],
  templateUrl: './manage-formulation.component.html',
  styleUrl: './manage-formulation.component.scss'
})
export class ManageFormulationComponent implements OnInit{
  addFormulation ?: Formulation | null;
  editFormulation ?: Formulation | null;
  formulations: Formulation[] = [];
  filteredFormulations: Formulation[] = [];
  paginatedFormulations: Formulation[] = [];
  showArchiveForm: number = 0;
  alertMessage : string = '';
  successMessage: string = '';

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  // Search property
  searchQuery: string = '';

  constructor(
    private getService : GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllFormulation();
  }

  getAllFormulation() {
    this.getService.getAllFormulationsAsync().subscribe({
      next: res => {
        this.formulations = res;
        this.filteredFormulations = [...this.formulations];
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  onSearchChange(): void {
    const query = this.searchQuery.trim().toLowerCase();

    if (query === '') {
      this.filteredFormulations = [...this.formulations];
    } else {
      this.filteredFormulations = this.formulations.filter((f) =>
        f.formulationName.toLowerCase().includes(query)
      );
    }

    this.currentPage = 1;
    this.updatePagination();
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }
  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveFormulation() {
    this.updateService.archiveFormulationByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllFormulation();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredFormulations.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedFormulations = this.filteredFormulations.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openAddFormulationForm() {
    this.addFormulation = new Formulation();
  }

  closeAddFormulationForm() {
    this.addFormulation = null;
    this.getAllFormulation();
  }

  openEditFormulationForm(formulation : Formulation) {
    this.editFormulation = formulation;
  }

  closeEditFormulationForm() {
    this.editFormulation = null;
  }
}