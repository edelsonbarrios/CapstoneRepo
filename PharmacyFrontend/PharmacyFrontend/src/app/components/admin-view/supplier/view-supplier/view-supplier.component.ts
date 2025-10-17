import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { FormsModule } from '@angular/forms';
import { NgIf, NgFor, CurrencyPipe } from '@angular/common';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-view-supplier',
  standalone: true,
  imports: [FormsModule, NgIf, NgFor],
  templateUrl: './view-supplier.component.html',
  styleUrl: './view-supplier.component.scss',
})
export class ViewSupplierComponent {
  @Input() viewSupplier: Supplier = new Supplier();
  @Output() closeForm = new EventEmitter<void>();
  isEditMode: boolean = false;
  showSuccessAlert: boolean = false;

  // Pagination properties
  currentPage: number = 1;
  pageSize: number = 3; // show 3 per page
  pagedProducts: any[] = [];

  constructor(private updateService: UpdateService) {}

  ngOnChanges() {
    if (this.viewSupplier && this.viewSupplier.products) {
      this.updatePagedProducts();
    }
  }

  get totalPages(): number {
    return Math.ceil(this.viewSupplier.products.length / this.pageSize);
  }

  updatePagedProducts() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedProducts = this.viewSupplier.products.slice(startIndex, endIndex);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagedProducts();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagedProducts();
    }
  }

  closeViewForm() {
    this.closeForm.emit();
  }

  toggleViewMode() {
    this.isEditMode = true;
  }

  saveChanges() {
    this.updateService
      .updateSupplierAsync(this.viewSupplier.id, this.viewSupplier)
      .subscribe({
        next: (res) => {
          this.showSuccessAlert = true;
        },
        error: (err) => {
          console.error(err);
          alert(JSON.stringify(err.error));
          console.log(JSON.stringify(err.error));
        },
      });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.isEditMode = false;
    this.closeForm.emit();
  }
}