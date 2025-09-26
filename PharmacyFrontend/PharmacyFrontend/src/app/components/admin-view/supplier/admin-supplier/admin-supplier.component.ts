import { Component, OnInit } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgIf } from '@angular/common';
import { CreateSupplierComponent } from '../create-supplier/create-supplier.component';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-admin-supplier',
  standalone: true,
  imports: [NgFor, CreateSupplierComponent, NgIf],
  templateUrl: './admin-supplier.component.html',
  styleUrl: './admin-supplier.component.scss'
})
export class AdminSupplierComponent implements OnInit{
  suppliers: Supplier[] = [];
  paginatedSuppliers: Supplier[] = [];
  newSupplier ?: Supplier | null;
  
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
        console.log(res);
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.suppliers.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedSuppliers = this.suppliers.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openCreateSupplierForm() {
    this.newSupplier = new Supplier();
  }

  closeCreateSupplierForm() {
    this.newSupplier = null;
    this.getAllSuppliers();
  }

  archiveSupplier(supplier : Supplier) {
    this.updateService.archiveSupplierAsync(supplier.id).subscribe({
      next: res => {
        alert(`${res.message}`);
        this.currentPage = 1;
        this.getAllSuppliers();
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }
}