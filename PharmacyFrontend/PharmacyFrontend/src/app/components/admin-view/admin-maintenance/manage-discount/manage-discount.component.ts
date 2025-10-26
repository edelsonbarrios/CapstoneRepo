import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { Discount } from '../../../../models/discount-models/discount';
import { CreateDiscountRequest } from '../../../../models/discount-models/create-discount';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { EditDiscountComponent } from '../edit-discount/edit-discount.component';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';

@Component({
  selector: 'app-manage-discount',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, EditDiscountComponent, ArchiveConfirmationComponent],
  templateUrl: './manage-discount.component.html',
  styleUrl: './manage-discount.component.scss',
})
export class ManageDiscountComponent implements OnInit {
  discounts: Discount[] = [];
  paginatedDiscounts: Discount[] = [];
  newDiscount: Discount = new Discount();
  discountError: string = '';
  editDiscount?: Discount | null;
  showSuccessAlert: boolean = false;
  successMessage: string = '';
  showArchiveForm: number = 0;
  errorMessage: string = '';

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private postService: PostService,
    private updateService: UpdateService
  ) { }

  ngOnInit(): void {
    this.getAllDiscount();
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  getAllDiscount() {
    this.getService.getAllDiscountAsync().subscribe({
      next: (discounts) => {
        this.discounts = discounts;
        this.updatePagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
        this.errorMessage = err.error;
      },
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.discounts.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedDiscounts = this.discounts.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  createNewDiscount() {
    if (!this.newDiscount.discountName) {
      this.discountError = 'Discount name is required';
      return;
    }
    if (!this.newDiscount.discountRate || this.newDiscount.discountRate <= 0) {
      this.discountError = "Rate must be greater than 0";
      return;
    }
    this.discountError = '';

    const requestPayload: CreateDiscountRequest = {
      discountName: this.newDiscount.discountName,
      discountRate: this.newDiscount.discountRate,
    };

    this.postService.createDiscountAsync(requestPayload).subscribe({
      next: (res) => {
        this.successMessage = 'Discount has been Added Successfully';
        this.showSuccessAlert = true;
      },
      error: (err) => {
        this.newDiscount.discountName = '';
        this.newDiscount.discountRate = 0;
        this.errorMessage = err.error;
      },
    });
  }

  closeErrrMessage() {
    this.errorMessage = '';
  }

  archiveDiscount() {
    this.updateService.archiveDiscountAsync(this.showArchiveForm).subscribe({
      next: mes => {
        this.successMessage = 'Discount has been Archived Successfully';
        this.showArchiveForm = 0;
        this.getAllDiscount();
      },
      error: err => {
        alert(`${err.error.message}`);
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.newDiscount.discountName = '';
    this.newDiscount.discountRate = 0;
    this.currentPage = 1;
    this.getAllDiscount();
  }

  openEditDiscountForm(discount: Discount) {
    this.editDiscount = { ...discount }; // Create a copy to avoid mutating the original
  }

  closeEditDiscountForm() {
    this.editDiscount = null;
    this.getAllDiscount(); // Refresh the list after editing
  }
}