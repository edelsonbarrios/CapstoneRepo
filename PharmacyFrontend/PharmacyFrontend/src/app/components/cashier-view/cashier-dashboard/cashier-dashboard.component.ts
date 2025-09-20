import { Component } from '@angular/core';
import { Product } from '../../../models/product-models/product';
import { NgIf } from '@angular/common';
import { ViewSettleComponent } from '../view-settle/view-settle.component';
import { PrintReceiptComponent } from '../print-receipt/print-receipt.component';

@Component({
  selector: 'app-cashier-dashboard',
  standalone: true,
  imports: [NgIf,ViewSettleComponent,PrintReceiptComponent],
  templateUrl: './cashier-dashboard.component.html',
  styleUrl: './cashier-dashboard.component.scss'
})
export class CashierDashboardComponent {
  viewSettle ?: Product | null;
  printReceipt ?: Product | null;

  openViewSettleForm(){
    this.viewSettle= new Product();
  }

  openPrintReceiptForm() {
    this.printReceipt= new Product();
  }
}
