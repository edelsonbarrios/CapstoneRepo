import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PurchaseOrder } from '../../../../models/purchase-order-models/purchase-order';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { PurchaseOrderProductsComponent } from '../purchase-order-products/purchase-order-products.component';
import { PurchaseOrderItem } from '../../../../models/purchase-order-item-models/purchase-order-item';
import { CurrencyPipe } from '@angular/common';
import { Product } from '../../../../models/product-models/product';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { CreatePurchaseOrder } from '../../../../models/purchase-order-models/create-purchase-order';
import { Employee } from '../../../../models/employee-models/employee';
import { LogInToken } from '../../../shared/token';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-inventory-purchase-order',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    PurchaseOrderProductsComponent,
    CurrencyPipe,
    NgFor,
  ],
  templateUrl: './inventory-purchase-order.component.html',
  styleUrl: './inventory-purchase-order.component.scss',
})
export class InventoryPurchaseOrderComponent implements OnInit {
  purchaseOrders: PurchaseOrder = new PurchaseOrder();
  openProductForm: boolean = false;
  suppliers: Supplier[] = [];
  employee: Employee = new Employee();

  // Pagination properties
  paginatedItems: PurchaseOrderItem[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getAllSuppliers();
    this.getLoggedInUser();
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.purchaseOrders.purchaseOrderItems.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.purchaseOrders.purchaseOrderItems.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openProductsForm() {
    this.openProductForm = true;
  }

  closeProductsForm() {
    this.openProductForm = false;
  }

  onProductsSelected(products: Product[]) {
    console.log(products);
    const items: PurchaseOrderItem[] = products.map((p) => ({
      id: 0,
      productId: p.productID,
      productName: p.productName,
      quantityOrdered: 1, // default, user can edit later
      quantityReceived: 0,
      unitCost: p.price,
      totalCost: p.price, // qty(1) * price
      batchNumber: '',
      manufacturingDate: new Date(),
      expirationDate: new Date(),
      category: p.categoryName,
      classification: p.classificationName,
      type: p.typeName
    }));

    this.purchaseOrders.purchaseOrderItems.push(...items);
    this.updatePagination();
    this.computeTotalAmount();
    this.openProductForm = false;
  }

  computeTotalAmount() {
    this.purchaseOrders.totalAmount =
      this.purchaseOrders.purchaseOrderItems.reduce(
        (sum, item) => sum + item.totalCost,
        0
      );
  }

  updateItemTotal(item: PurchaseOrderItem) {
    item.totalCost = item.quantityOrdered * item.unitCost;
    this.computeTotalAmount();
  }

  removeItem(index: number) {
    this.purchaseOrders.purchaseOrderItems.splice(index, 1);
    this.updatePagination();
    this.computeTotalAmount();
  }

  // New method for removing items with pagination
  removeItemPaginated(item: PurchaseOrderItem) {
    const index = this.purchaseOrders.purchaseOrderItems.findIndex(i => i === item);
    if (index !== -1) {
      this.purchaseOrders.purchaseOrderItems.splice(index, 1);
      
      // Adjust current page if necessary
      if (this.paginatedItems.length === 1 && this.currentPage > 1) {
        this.currentPage--;
      }
      
      this.updatePagination();
      this.computeTotalAmount();
    }
  }

  createPurchaseOrder() {
    const requestPayload: CreatePurchaseOrder = {
      purchaseOrderNumber: this.purchaseOrders.purchaseOrderNumber,
      orderDate: new Date(this.purchaseOrders.orderDate).toISOString(),
      deliveryDate: new Date(this.purchaseOrders.deliveryDate).toISOString(),
      employeeLastName: this.employee.lastName,
      supplierName: this.purchaseOrders.supplierName,
      notes: this.purchaseOrders.notes,
      purchaseOrderItems: this.purchaseOrders.purchaseOrderItems.map(
        (item) => ({
          productId: item.productId,
          quantityOrdered: item.quantityOrdered,
          unitCost: item.unitCost,
          batchNumber: item.batchNumber,
          manufacturingDate: new Date(item.manufacturingDate).toISOString(),
          expirationDate: new Date(item.expirationDate).toISOString(),
        })
      ),
    };

    this.postService.createPurchaseOrderAsync(requestPayload).subscribe({
      next: (res) => {
        alert(`Purchase Order successfully`);
        this.clearFields();
      },
      error: (err) => {
        console.log(`${err.error}`);
        console.log(this.purchaseOrders);
        console.log(requestPayload);
      },
    });
  }

  clearFields() {
    this.purchaseOrders = new PurchaseOrder(); // resets everything
    this.purchaseOrders.purchaseOrderItems = []; // ensure items array cleared
    this.currentPage = 1;
    this.updatePagination();
  }

  getAllSuppliers() {
    this.getService.getAllSuppliersAsync().subscribe({
      next: (res) => {
        this.suppliers = res;
      },
      error: (err) => {
        console.log('Has an error getting all the suppliers.');
        console.log(`${err.error}`);
      },
    });
  }

  getLoggedInUser() {
    var token = localStorage.getItem(LogInToken);
    if (!token) return;

    var employee: any = jwtDecode(token);

    const employeeId = parseInt(employee.EmployeeId);
    if (!employeeId) console.log(`Employee Id does not exist.`);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (res) => {
        this.employee = res;
      },
      error: (err) => {
        console.log(`Logged-in user does not exist.`);
      },
    });
  }
}