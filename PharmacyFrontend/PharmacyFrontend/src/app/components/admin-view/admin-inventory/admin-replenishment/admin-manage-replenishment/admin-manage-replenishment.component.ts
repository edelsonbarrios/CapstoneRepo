import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { PurchaseOrder } from '../../../../../models/purchase-order-models/purchase-order';
import { GetService } from '../../../../../services/GetService/get.service';
import { ViewPurchaseOrderDetailsComponent } from '../../../../inventory-manager-view/inventory-manager-replenishment/view-purchase-order-details/view-purchase-order-details.component';
import { ArchiveConfirmationComponent } from '../../../../shared/archive-confirmation/archive-confirmation.component';
import { UpdateService } from '../../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-admin-manage-replenishment',
  standalone: true,
  imports: [
    FormsModule,
    NgFor,
    DatePipe,
    ViewPurchaseOrderDetailsComponent,
    NgIf,
    ArchiveConfirmationComponent,
  ],
  templateUrl: './admin-manage-replenishment.component.html',
  styleUrl: './admin-manage-replenishment.component.scss'
})
export class ManageReplenishmentComponent implements OnInit {
  purchaseOrders: PurchaseOrder[] = [];
  paginatedPurchaseOrders: PurchaseOrder[] = [];
  purchaseOrder?: PurchaseOrder | null;
  showArchiveForm: number = 0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllPendingPurchaseOrders();
  }

  getAllPendingPurchaseOrders() {
    this.getService.getAllPendingPurchaseOrdersAsync().subscribe({
      next: (res) => {
        this.purchaseOrders = res;
        this.updatePagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.purchaseOrders.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedPurchaseOrders = this.purchaseOrders.slice(
      startIndex,
      endIndex
    );
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  viewPurchaseOrderDetails(purchaseOrder: PurchaseOrder) {
    this.purchaseOrder = purchaseOrder;
  }

  openArchivePurchaseOrderForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchivePurchaseOrderForm() {
    this.showArchiveForm = 0;
  }

  archivePurchaseOrder() {
    this.updateService
      .archivePurchaseOrderByIdAsync(this.showArchiveForm)
      .subscribe({
        next: (res) => {
          this.showArchiveForm = 0;
          this.getAllPendingPurchaseOrders();
        },
        error: (err) => {
          console.log(`${err.error}`);
          console.log(this.showArchiveForm);
        },
      });
  }

  closeViewPurchaseOrderForm() {
    this.purchaseOrder = null;
    this.getAllPendingPurchaseOrders();

    setTimeout(() => {
      this.purchaseOrder = null;
    }, 300);
  }
}
