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

@Component({
  selector: 'app-manage-discount',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, EditDiscountComponent, ArchiveConfirmationComponent, DatePipe],
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

// ===================================
// manage-discount.component.html
// ===================================
/*
<div class="wrapper">
  <div class="manage-product">
    <h1>Manage Discount</h1>

    <!-- DISCOUNT INPUT CONTROLS -->
    <div class="product-control">
      <div class="input-wrapper">
        <!-- Discount Name -->
        <input
          type="text"
          id="discount-name"
          name="discount-name"
          class="input-field"
          placeholder="Discount Name"
          [(ngModel)]="newDiscount.discountName"
          [class.error]="discountError">

        <!-- Discount Type Dropdown -->
        <select
          id="discount-type"
          name="discount-type"
          class="input-field select-field"
          [(ngModel)]="selectedDiscountType"
          (change)="onDiscountTypeChange()"
          [class.error]="discountError">
          <option *ngFor="let type of discountTypes" [value]="type">
            {{ type }}
          </option>
        </select>

        <!-- Discount Value -->
        <input
          type="number"
          id="value"
          name="value"
          class="input-field"
          [placeholder]="getValueLabel()"
          [(ngModel)]="newDiscount.discountAmount"
          [class.error]="discountError"
          min="0"
          [max]="selectedDiscountType === 'Percentage' ? 100 : 999999"
          step="0.01">

        <!-- Start Date -->
        <input
          type="date"
          id="start-date"
          name="start-date"
          class="input-field date-field"
          [(ngModel)]="newDiscount.startDate"
          [class.error]="discountError">

        <!-- End Date -->
        <input
          type="date"
          id="end-date"
          name="end-date"
          class="input-field date-field"
          [(ngModel)]="newDiscount.endDate"
          [class.error]="discountError">
      </div>

      <button class="btn-product" (click)="createNewDiscount()">Save Discount</button>
    </div>
    <p class="error-message" *ngIf="discountError">{{ discountError }}</p>

    <!-- DISCOUNT TABLE -->
    <table class="product-table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Type</th>
          <th>Value</th>
          <th>Start Date</th>
          <th>End Date</th>
          <th>Status</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngIf="discounts.length === 0">
          <td colspan="7" style="text-align: center; color: #94a3b8; padding: 2rem;">
            No recent data
          </td>
        </tr>
        <tr *ngFor="let d of paginatedDiscounts">
          <td>{{ d.discountName }}</td>
          <td>
            <span class="badge" [class.badge-percentage]="d.discountType === 'Percentage'"
                  [class.badge-fixed]="d.discountType === 'Fixed'">
              {{ d.discountType }}
            </span>
          </td>
          <td>{{ formatDiscountValue(d) }}</td>
          <td>{{ formatDate(d.startDate) }}</td>
          <td>{{ formatDate(d.endDate) }}</td>
          <td>
            <span class="status-badge active">Active</span>
          </td>
          <td>
            <button class="btn-view" (click)="openEditDiscountForm(d)">Edit</button>
            <button class="btn-archive" (click)="openArchiveForm(d.id)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- PAGINATION CONTROLS -->
    <div class="pagination-controls" *ngIf="discounts.length > 0">
      <button class="btn-page" [disabled]="currentPage === 1" (click)="goToPage(currentPage - 1)">Previous</button>
      <span class="page-info">Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="btn-page" [disabled]="currentPage === totalPages" (click)="goToPage(currentPage + 1)">Next</button>
    </div>
  </div>
</div>

<!-- Edit Discount Modal -->
<div class="modal-overlay" *ngIf="editDiscount">
  <app-edit-discount
    [editDiscount]="editDiscount"
    (closeForm)="closeEditDiscountForm()">
  </app-edit-discount>
</div>

<!-- Success Alert Modal -->
<div class="modal-overlay" *ngIf="showSuccessAlert">
  <div class="success-alert">
    <div class="alert-icon success">
      <svg width="48" height="48" viewBox="0 0 48 48" fill="none">
        <circle cx="24" cy="24" r="22" fill="#10b981" opacity="0.1"/>
        <circle cx="24" cy="24" r="18" fill="#10b981"/>
        <path d="M16 24L21 29L32 18" stroke="white" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"/>
      </svg>
    </div>
    <h3 class="alert-title">Success!</h3>
    <p class="alert-message">{{ successMessage }}</p>
    <div class="alert-footer">
      <button class="ok-btn" (click)="closeSuccessAlert()">Got it</button>
    </div>
  </div>
</div>

<!-- Archive Confirmation Modal -->
<div class="modal-overlay" *ngIf="showArchiveForm">
  <app-archive-confirmation
    *ngIf="showArchiveForm"
    [id]="showArchiveForm"
    [alertMessage]="'Discount successfully deleted.'"
    [message]="'Do you really want to delete this discount?'"
    (onArchive)="archiveDiscount()"
    (onCancel)="closeArchiveForm()">
  </app-archive-confirmation>
</div>

<!-- Error Alert Modal -->
<div class="modal-overlay" *ngIf="errorMessage">
  <div class="success-alert">
    <div class="alert-icon error">
      <svg width="48" height="48" viewBox="0 0 48 48" fill="none">
        <circle cx="24" cy="24" r="22" fill="#ef4444" opacity="0.1"/>
        <circle cx="24" cy="24" r="18" fill="#ef4444"/>
        <path d="M28 20L20 28M20 20L28 28" stroke="white" stroke-width="3" stroke-linecap="round"/>
      </svg>
    </div>
    <h3 class="alert-title">Error</h3>
    <p class="alert-message">{{ errorMessage }}</p>
    <div class="alert-footer">
      <button class="ok-btn" (click)="closeErrrMessage()">OK</button>
    </div>
  </div>
</div>
*/
