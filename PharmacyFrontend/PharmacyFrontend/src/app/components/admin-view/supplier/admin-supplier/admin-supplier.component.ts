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
  newSupplier ?: Supplier | null;

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
        console.log(res);
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
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
        this.getAllSuppliers();
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }
}
