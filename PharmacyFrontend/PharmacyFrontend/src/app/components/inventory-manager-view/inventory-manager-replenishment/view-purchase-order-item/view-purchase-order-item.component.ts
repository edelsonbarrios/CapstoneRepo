import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PurchaseOrderItem } from '../../../../models/purchase-order-item-models/purchase-order-item';
import { GetService } from '../../../../services/GetService/get.service';
import { Product } from '../../../../models/product-models/product';

@Component({
  selector: 'app-view-purchase-order-item',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './view-purchase-order-item.component.html',
  styleUrl: './view-purchase-order-item.component.scss'
})
export class ViewPurchaseOrderItemComponent implements OnInit {
  @Input() item: PurchaseOrderItem = new PurchaseOrderItem();
  @Output() closeModal = new EventEmitter<void>();
  @Output() saveItem = new EventEmitter<PurchaseOrderItem>();
  product: Product = new Product();
  errorMessage : string = '';

  constructor(
    private getService : GetService,
  ) {}

  // Local copy to prevent direct mutation until save
  localItem: PurchaseOrderItem = new PurchaseOrderItem();

  ngOnInit(): void {
    // Create a deep copy of the item
    this.localItem = { ...this.item };
  }

  validateMfgDate() {
    if (!this.localItem.manufacturingDate) {
      this.localItem.mfgDateError = '';
      return;
    }

    const mfgDate = new Date(this.localItem.manufacturingDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const yearsAgo = new Date(today);
    yearsAgo.setFullYear(today.getFullYear() - 5);

    if (mfgDate > today) {
      this.localItem.mfgDateError = 'Manufacturing date cannot be in the future';
    } else if (mfgDate < yearsAgo) {
      this.localItem.mfgDateError = 'Manufacturing date is too old (>5 years)';
    } else {
      this.localItem.mfgDateError = '';
    }
  }

  validateExpDate() {
    if (!this.localItem.expirationDate) {
      this.localItem.expDateError = '';
      return;
    }

    const expDate = new Date(this.localItem.expirationDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (expDate <= today) {
      this.localItem.expDateError = 'Expiration date must be in the future';
    } else {
      this.localItem.expDateError = '';
    }
  }

  getProductInfo() {
    this.getService.getProductByIdAsync(this.localItem.productId).subscribe({
      next: res => {
        this.product = res;
      },
      error: err => {
        console.log(`Product does not exist.`);
      }
    });
  }

  calculateTotal() {
    const quantityReceived = this.localItem.quantityReceived || 0;
    const damagedPiecesCount = this.localItem.damagedPiecesCount || 0;
    const expiredPiecesCount = this.localItem.expiredPiecesCount || 0;
    const defectPiecesCount = this.localItem.defectPiecesCount || 0;

    // Calculate base total based on unit type
    let baseTotal = 0;
    if (this.localItem.poUnitType === 'Box') {
      baseTotal = quantityReceived * this.localItem.boxPrice;
    } else {
      baseTotal = quantityReceived * this.localItem.piecePrice;
    }

    // Deduct all damaged/expired/defect pieces (at piece price)
    const totalDeductions = (damagedPiecesCount + expiredPiecesCount + defectPiecesCount) * this.localItem.piecePrice;

    // Calculate final total
    const finalTotal = baseTotal - totalDeductions;

    return finalTotal < 0 ? 0 : finalTotal;
  }

  getDisplayUnitPrice(): number {
    return this.localItem.poUnitType === 'Box' ? this.localItem.boxPrice : this.localItem.piecePrice;
  }

  onSave() {
    // Validate all fields
    this.validateMfgDate();
    this.validateExpDate();

    if (this.localItem.mfgDateError || this.localItem.expDateError) {
      this.errorMessage = 'Fix the Manufacturing Date & Expiration Date';
      return;
    }

    if(this.localItem.quantityReceived <= 0 || !this.localItem.quantityReceived) {
      this.errorMessage = `Quantity Received of ${this.localItem.productName} is 0`;
      return;
    }

    // Update total cost
    this.localItem.totalCost = this.calculateTotal();

    if(this.localItem.totalCost <= 0)  {
      this.errorMessage = 'There is no more pieces left. Please try again.';
      return;
    }

    // Emit the updated item
    this.saveItem.emit(this.localItem);
    console.log(`${JSON.stringify(this.localItem)}`);
  }

  closeErrorMessageForm() {
    this.errorMessage = '';
  }

  onCancel() {
    this.closeModal.emit();
  }
}
