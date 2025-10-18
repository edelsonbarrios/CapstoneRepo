import { Component, OnInit, Type } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddTypeComponent } from '../add-type/add-type.component';
import { EditTypeComponent } from '../edit-type/edit-type.component';
import { TypeModel } from '../../../../models/type-models/type';
import { GetService } from '../../../../services/GetService/get.service';
import { NgForOf } from "../../../../../../node_modules/@angular/common/index";
import { ArchiveConfirmationComponent } from "../../../shared/archive-confirmation/archive-confirmation.component";
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-manage-type',
  standalone: true,
  imports: [NgIf, AddTypeComponent, EditTypeComponent, NgFor, ArchiveConfirmationComponent],
  templateUrl: './manage-type.component.html',
  styleUrl: './manage-type.component.scss'
})
export class ManageTypeComponent implements OnInit{
  addType ?: TypeModel | null;
  editType ?: TypeModel | null;
  types : TypeModel[] = [];
  paginatedTypes: TypeModel[] = [];
  showArchiveForm: number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllTypes();
  }

  getAllTypes() {
    this.getService.getAllTypesAsync().subscribe({
      next: res => {
        this.types = res;
        this.updatePagination();
      },
      error : err => {
        console.log(`${err.error}`);
      }
    });
  }

  openArchiveConfirmationForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveConfirmationForm() {
    this.showArchiveForm = 0;
  }

  archiveType() {
    this.updateService.archiveTypeModelByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllTypes();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.types.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedTypes = this.types.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openAddTypeForm() {
    this.addType = new TypeModel();
  }

  closeAddTypeForm() {
    this.addType = null;
    this.getAllTypes();
  }

  openEditTypeForm(type : TypeModel) {
    this.editType = type;
  }

  closeEditTypeForm() {
    this.editType = null;
  }
}