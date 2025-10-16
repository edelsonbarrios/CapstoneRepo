import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddGenericComponent } from '../add-generic/add-generic.component';
import { EditGenericComponent } from '../edit-generic/edit-generic.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Generic } from '../../../../models/generic-models/generic';
import { FormsModule } from '@angular/forms';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-manage-generic-name',
  standalone: true,
  imports: [
    NgIf,
    AddGenericComponent,
    EditGenericComponent,
    FormsModule,
    NgFor,
    ArchiveConfirmationComponent
  ],
  templateUrl: './manage-generic-name.component.html',
  styleUrl: './manage-generic-name.component.scss',
})
export class ManageGenericNameComponent implements OnInit {
  addGeneric?: Generic | null;
  editGeneric?: Generic | null;
  generics: Generic[] = [];
  paginatedGenerics: Generic[] = [];
  showArchiveForm : number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllGenerics();
  }

  getAllGenerics() {
    this.getService.getAllGenericsAsync().subscribe({
      next: (res) => {
        this.generics = res;
        this.updatePagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  openArchiveConfirmationForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveConfirmationForm() {
    this.showArchiveForm = 0;
  }

  archiveGeneric() {
    if(this.showArchiveForm === 0 || !this.showArchiveForm) return;
    this.updateService.archiveGenericByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0
        this.getAllGenerics();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.generics.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedGenerics = this.generics.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openAddGenericForm() {
    this.addGeneric = new Generic();
  }

  closeAddGenericForm() {
    this.addGeneric = null;
    this.getAllGenerics();
  }

  openEditGenericForm(generic: Generic) {
    this.editGeneric = generic;
  }

  closeEditGenericForm() {
    this.editGeneric = null;
  }
}