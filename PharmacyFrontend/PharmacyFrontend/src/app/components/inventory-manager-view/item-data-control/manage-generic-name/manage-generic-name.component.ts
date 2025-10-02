import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddGenericComponent } from '../add-generic/add-generic.component';
import { EditGenericComponent } from '../edit-generic/edit-generic.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Generic } from '../../../../models/generic-models/generic';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-generic-name',
  standalone: true,
  imports: [
    NgIf,
    AddGenericComponent,
    EditGenericComponent,
    FormsModule,
    NgFor,
  ],
  templateUrl: './manage-generic-name.component.html',
  styleUrl: './manage-generic-name.component.scss',
})
export class ManageGenericNameComponent implements OnInit {
  addGeneric?: Generic | null;
  editGeneric?: Generic | null;
  generics: Generic[] = [];
  paginatedGenerics: Generic[] = [];

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(private getService: GetService) {}

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