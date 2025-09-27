import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddBrandComponent } from '../add-brand/add-brand.component';
import { EditBrandComponent } from '../edit-brand/edit-brand.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Brand } from '../../../../models/brand-models/brand';

@Component({
  selector: 'app-manage-brand-name',
  standalone: true,
  imports: [NgIf, AddBrandComponent, EditBrandComponent, NgFor],
  templateUrl: './manage-brand-name.component.html',
  styleUrl: './manage-brand-name.component.scss',
})
export class ManageBrandNameComponent implements OnInit{
  addBrand?: Brand | null;
  editBrand?: Brand | null;
  brands: Brand[] = [];
  paginatedBrands: Brand[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(private getService: GetService) {}

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