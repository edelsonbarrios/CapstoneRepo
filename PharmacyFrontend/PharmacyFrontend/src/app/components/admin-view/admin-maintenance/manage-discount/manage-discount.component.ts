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
import { DatePipe } from '@angular/common';
import { AssignInventoryDiscountComponent } from '../assign-inventory-discount/assign-inventory-discount.component';

@Component({
  selector: 'app-manage-discount',
  standalone: true,
  imports: [FormsModule,
    NgFor,
    NgIf,
    EditDiscountComponent,
    ArchiveConfirmationComponent,
    DatePipe,
    AssignInventoryDiscountComponent
  ],
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
  showAssignModal: boolean = false;
  selectedDiscountForAssign?: Discount;

  // Discount type options
  discountTypes: string[] = ['Percentage', 'Fixed'];
  selectedDiscountType: string = 'Percentage';

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
    this.initializeNewDiscount();
  }

  initializeNewDiscount() {
    this.newDiscount = new Discount();
    this.newDiscount.discountType = 'Percentage';
    this.selectedDiscountType = 'Percentage';
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  openAssignModal(discount: Discount) {
    this.selectedDiscountForAssign = discount;
    this.showAssignModal = true;
  }

  closeAssignModal() {
    this.showAssignModal = false;
    this.selectedDiscountForAssign = undefined;
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

  onDiscountTypeChange() {
    this.newDiscount.discountType = this.selectedDiscountType;
  }

  createNewDiscount() {
    // Validation
    if (!this.newDiscount.discountName) {
      this.discountError = 'Discount name is required';
      return;
    }

    if (!this.newDiscount.discountAmount || this.newDiscount.discountAmount <= 0) {
      this.discountError = "Value must be greater than 0";
      return;
    }

    // Validate percentage range
    if (this.selectedDiscountType === 'Percentage' && this.newDiscount.discountAmount > 100) {
      this.discountError = "Percentage cannot exceed 100%";
      return;
    }

    // Validate dates if both are provided
    if (this.newDiscount.startDate && this.newDiscount.endDate) {
      const startDate = new Date(this.newDiscount.startDate);
      const endDate = new Date(this.newDiscount.endDate);

      if (endDate < startDate) {
        this.discountError = "End date cannot be before start date";
        return;
      }
    }

    this.discountError = '';

    const requestPayload: CreateDiscountRequest = {
      discountName: this.newDiscount.discountName,
      discountType: this.selectedDiscountType,
      discountAmount: this.newDiscount.discountAmount,
      startDate: this.newDiscount.startDate && this.newDiscount.startDate !== ''
        ? new Date(this.newDiscount.startDate).toISOString()
        : undefined,
      endDate: this.newDiscount.endDate && this.newDiscount.endDate !== ''
        ? new Date(this.newDiscount.endDate).toISOString()
        : undefined
    };

    this.postService.createDiscountAsync(requestPayload).subscribe({
      next: (res) => {
        this.successMessage = 'Discount has been added successfully';
        this.showSuccessAlert = true;
      },
      error: (err) => {
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
        this.successMessage = 'Discount has been deleted successfully';
        this.showArchiveForm = 0;
        this.getAllDiscount();
      },
      error: err => {
        this.errorMessage = err.error.message || 'Failed to delete discount';
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.initializeNewDiscount();
    this.currentPage = 1;
    this.getAllDiscount();
  }

  openEditDiscountForm(discount: Discount) {
    this.editDiscount = { ...discount };
  }

  closeEditDiscountForm() {
    this.editDiscount = null;
    this.getAllDiscount();
  }

  getValueLabel(): string {
    return this.selectedDiscountType === 'Percentage' ? 'Value (%)' : 'Value (₱)';
  }

  formatDiscountValue(discount: Discount): string {
    if (discount.discountType === 'Percentage') {
      return `${discount.discountAmount}%`;
    } else {
      return `₱${discount.discountAmount.toFixed(2)}`;
    }
  }

  formatDate(date: Date | null | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
}


