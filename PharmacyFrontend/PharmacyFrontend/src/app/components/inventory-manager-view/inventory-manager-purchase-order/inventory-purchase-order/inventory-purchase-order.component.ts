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
  selectedSupplier: Supplier = new Supplier();

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
    this.totalPages = Math.ceil(
      this.purchaseOrders.purchaseOrderItems.length / this.itemsPerPage
    );
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.purchaseOrders.purchaseOrderItems.slice(
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

  openProductsForm() {
    if (!this.selectedSupplier) {
      alert('Please select a supplier first');
      return;
    }
    this.openProductForm = true;
    console.log(this.selectedSupplier);
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
      boxQuantityOrdered: 1, // default, user can edit later
      boxQuantityReceived: 0,
      sheetQuantityOrdered: 1,
      sheetQuantityReceived: 0,
      pieceQuantityOrdered: 1,
      pieceQuantityReceived: 0,
      boxPrice: p.boxPrice,
      sheetPrice: p.sheetPrice,
      piecePrice: p.piecePrice,
      totalCost: p.boxPrice, // qty(1) * price
      batchNumber: '',
      manufacturingDate: new Date(),
      expirationDate: new Date(),
      category: p.categoryName,
      classification: p.classificationName,
      type: p.typeName,
      unitMeasurement: p.unitMeasurement,
    }));

    this.purchaseOrders.purchaseOrderItems.push(...items);
    console.log(items);
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
    item.totalCost = item.boxQuantityOrdered * item.boxPrice;
    this.computeTotalAmount();
  }

  removeItem(index: number) {
    this.purchaseOrders.purchaseOrderItems.splice(index, 1);
    this.updatePagination();
    this.computeTotalAmount();
  }

  // New method for removing items with pagination
  removeItemPaginated(item: PurchaseOrderItem) {
    const index = this.purchaseOrders.purchaseOrderItems.findIndex(
      (i) => i === item
    );
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
      supplierName: this.selectedSupplier.supplierName,
      notes: this.purchaseOrders.notes,
      purchaseOrderItems: this.purchaseOrders.purchaseOrderItems.map(
        (item) => ({
          productId: item.productId,
          boxQuantityOrdered: item.boxQuantityOrdered,
          boxQuantityReceived: item.boxQuantityReceived,
          sheetQuantityOrdered: item.sheetQuantityOrdered,
          sheetQuantityReceived: item.sheetQuantityReceived,
          pieceQuantityOrdered: item.pieceQuantityOrdered,
          pieceQuantityReceived: item.pieceQuantityReceived,
          boxPrice: item.boxPrice,
          sheetPrice: item.sheetPrice,
          piecePrice: item.piecePrice,
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
        console.log(requestPayload);
      },
    });
  }

  recalculateQuantitiesAndTotals(item: PurchaseOrderItem) {
    const product = this.selectedSupplier.products.find(
      (p) => p.productID === item.productId
    );
    if (!product) return;

    // Reset defaults
    item.sheetQuantityOrdered = 0;
    item.pieceQuantityOrdered = 0;
    item.sheetPrice = 0;
    item.piecePrice = 0;

    // Dual-level: Box → Piece
    if (product.unitMeasurement === 'Dual-Level') {
      item.pieceQuantityOrdered =
        item.boxQuantityOrdered * (product.sheetsPerBox || 1); // here sheetsPerBox acts as piecesPerBox

      // piece price = boxPrice / piecesPerBox
      item.piecePrice =
        product.sheetsPerBox && product.sheetsPerBox > 0
          ? product.boxPrice / product.sheetsPerBox
          : product.piecePrice;
    }

    // Triple-level: Box → Sheet → Piece
    else if (product.unitMeasurement === 'Multi-Level') {
      item.sheetQuantityOrdered =
        item.boxQuantityOrdered * product.sheetsPerBox;
      item.pieceQuantityOrdered =
        item.sheetQuantityOrdered * product.piecesPerSheet;

      // Derived prices
      item.sheetPrice =
        product.sheetsPerBox && product.sheetsPerBox > 0
          ? product.boxPrice / product.sheetsPerBox
          : product.sheetPrice;

      item.piecePrice =
        product.piecesPerSheet && product.piecesPerSheet > 0
          ? item.sheetPrice / product.piecesPerSheet
          : product.piecePrice;
    }

    // Total cost based on box quantity
    item.totalCost = item.boxQuantityOrdered * item.boxPrice;

    this.computeTotalAmount();
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
