import { Component, OnInit } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { AddBrandComponent } from '../add-brand/add-brand.component';
import { EditBrandComponent } from '../edit-brand/edit-brand.component';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { GetService } from '../../../../services/GetService/get.service';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { Brand } from '../../../../models/brand-models/brand';

@Component({
  selector: 'app-manage-brand-name',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    AddBrandComponent,
    EditBrandComponent,
    ArchiveConfirmationComponent,
  ],
  templateUrl: './manage-brand-name.component.html',
  styleUrl: './manage-brand-name.component.scss',
})
export class ManageBrandNameComponent implements OnInit {
  addBrand?: Brand | null;
  editBrand?: Brand | null;
  brands: Brand[] = [];
  filteredBrands: Brand[] = [];
  paginatedBrands: Brand[] = [];
  showArchiveForm: number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  // Search property
  searchQuery: string = '';

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllBrands();
  }

  getAllBrands() {
    this.getService.getAllBrandsAsync().subscribe({
      next: (res) => {
        this.brands = res;
        this.filteredBrands = [...this.brands]; // keep unfiltered copy
        this.updatePagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  // 🔍 Search functionality
  onSearchChange(): void {
    const query = this.searchQuery.trim().toLowerCase();

    if (query === '') {
      this.filteredBrands = [...this.brands];
    } else {
      this.filteredBrands = this.brands.filter(
        (b) =>
          b.brandName.toLowerCase().includes(query));
    }

    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredBrands.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedBrands = this.filteredBrands.slice(startIndex, endIndex);
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

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveBrand() {
    if (this.showArchiveForm == 0 || !this.showArchiveForm) return;
    this.updateService.archiveBrandByIdAsync(this.showArchiveForm).subscribe({
      next: (res) => {
        this.showArchiveForm = 0;
        this.getAllBrands();
      },
      error: (err) => {
        console.log(`${err.error.message}`);
      },
    });
  }
}
