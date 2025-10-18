import { Component, OnInit } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgIf } from '@angular/common';
import { CreateSupplierComponent } from '../create-supplier/create-supplier.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { AssignProductsSupplierComponent } from "../assign-products-supplier/assign-products-supplier.component";
import { ViewSupplierComponent } from "../view-supplier/view-supplier.component";
import { ArchiveConfirmationComponent } from "../../../shared/archive-confirmation/archive-confirmation.component";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-supplier',
  standalone: true,
  imports: [
    NgFor, 
    CreateSupplierComponent, 
    NgIf, 
    AssignProductsSupplierComponent, 
    ViewSupplierComponent, 
    ArchiveConfirmationComponent,
    FormsModule
  ],
  templateUrl: './admin-supplier.component.html',
  styleUrl: './admin-supplier.component.scss'
})
export class AdminSupplierComponent implements OnInit{
  suppliers: Supplier[] = [];
  paginatedSuppliers: Supplier[] = [];
  newSupplier ?: Supplier | null;
  assignSupplier?: Supplier | null;
  viewSupplier?: Supplier | null;
  showSuccessAlert: boolean = false;
  showArchiveForm : number = 0;
  supplierToArchive: Supplier = new Supplier();
  searchQuery: string = '';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService,
    private updateService : UpdateService
  ){}

  ngOnInit(): void {
      this.getAllSuppliers();
  }

  getAllSuppliers() {
    this.getService.getAllSuppliersAsync().subscribe({
      next: res => {
        this.suppliers = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  get getFilteredSuppliers() : Supplier[] {
    if(!this.searchQuery.trim()) {
      return this.suppliers;
    }
    const query = this.searchQuery.toLowerCase();
    return this.suppliers.filter(s => s.supplierName.toLowerCase().includes(query))
  }

  updatePagination() {
    const items = this.getFilteredSuppliers;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedSuppliers = items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  onSearchChange() {
  this.currentPage = 1;
  this.updatePagination();
}

  openCreateSupplierForm() {
    this.newSupplier = new Supplier();
  }

  closeCreateSupplierForm() {
    this.newSupplier = null;
    this.getAllSuppliers();
  }

  openAssignProductSupplierForm(supplier: Supplier) {
    this.assignSupplier = supplier;
  }
  
  closeAssignProductSupplierForm() {
    this.assignSupplier = null;
    this.getAllSuppliers();
  }
  
  openViewSupplierForm(supplier : Supplier) {
    this.viewSupplier = supplier;
  }

  closeViewSupplierForm() {
    this.viewSupplier = null;
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  getSupplierById(id: number) : Supplier{
    this.getService.getSupplierByIdAsync(id).subscribe({
      next: res => {
        return res;
      },
      error: err => {
        console.log(`${err.error}`);
        return null;
      }
    });
    return new Supplier();
  }

  archiveSupplier() {
    this.updateService.archiveSupplierAsync(this.showArchiveForm).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllSuppliers();
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.currentPage = 1;
    this.getAllSuppliers();
  }
}