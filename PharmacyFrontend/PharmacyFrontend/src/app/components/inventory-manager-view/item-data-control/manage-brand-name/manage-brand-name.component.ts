import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddBrandComponent } from '../add-brand/add-brand.component';
import { EditBrandComponent } from '../edit-brand/edit-brand.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Brand } from '../../../../models/brand-models/brand';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-manage-brand-name',
  standalone: true,
  imports: [
    NgIf, 
    AddBrandComponent, 
    EditBrandComponent, 
    NgFor,
    ArchiveConfirmationComponent
  ],
  templateUrl: './manage-brand-name.component.html',
  styleUrl: './manage-brand-name.component.scss',
})
export class ManageBrandNameComponent implements OnInit{
  addBrand?: Brand | null;
  editBrand?: Brand | null;
  brands: Brand[] = [];
  paginatedBrands: Brand[] = [];
  showArchiveForm : number = 0;
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllBrands();
  }

  getAllBrands() {
    this.getService.getAllBrandsAsync().subscribe({
      next: (res) => {
        this.brands = res;
        this.updatePagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveBrand() {
    if(this.showArchiveForm == 0 || !this.showArchiveForm) return;
    this.updateService.archiveBrandByIdAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllBrands();
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.brands.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedBrands = this.brands.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openAddBrandForm() {
    this.addBrand = new Brand();
  }

  closeAddBrandForm() {
    this.addBrand = null;
    this.getAllBrands();
  }

  openEditBrandForm(brandToEdit: Brand) {
    this.editBrand = brandToEdit;
  }

  closeEditBrandForm() {
    this.editBrand = null;
  }
}