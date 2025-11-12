import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { PurchaseOrder } from '../../../../../models/purchase-order-models/purchase-order';
import { GetService } from '../../../../../services/GetService/get.service';
import { PostService } from '../../../../../services/PostService/post.service';
import { PurchaseOrderProductsComponent } from '../../../../inventory-manager-view/inventory-manager-purchase-order/purchase-order-products/purchase-order-products.component';
import { PurchaseOrderItem } from '../../../../../models/purchase-order-item-models/purchase-order-item';
import { Product } from '../../../../../models/product-models/product';
import { Supplier } from '../../../../../models/supplier-models/supplier';
import { CreatePurchaseOrder } from '../../../../../models/purchase-order-models/create-purchase-order';
import { Employee } from '../../../../../models/employee-models/employee';
import { LogInToken } from '../../../../shared/token';

@Component({
  selector: 'app-admin-inventory-purchase-order',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    PurchaseOrderProductsComponent,
    CurrencyPipe,
    NgFor,
  ],
  templateUrl: './admin-inventory-purchase-order.component.html',
  styleUrl: './admin-inventory-purchase-order.component.scss'
})
export class InventoryPurchaseOrderComponent implements OnInit {
  purchaseOrders: PurchaseOrder = new PurchaseOrder();
  openProductForm: boolean = false;
  suppliers: Supplier[] = [];
  employee: Employee = new Employee();
  selectedSupplier: Supplier = new Supplier();
  purchaseOrderNumber: string = '';
  showSuccessAlert: boolean = false;
  selectSupplier: boolean = false;
  addItem: boolean = false;

  // Search filter
  searchQuery: string = '';

  paginatedItems: PurchaseOrderItem[] = [];
  currentPage = 1;
  itemsPerPage = 5;
  totalPages = 0;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) { }

  ngOnInit(): void {
    this.getAllSuppliers();
    this.getLoggedInUser();
    this.generateNewPurchaseOrderNumber();
    this.updatePagination();
  }

  // Filtered items based on search query
  get filteredItems(): PurchaseOrderItem[] {
    if (!this.searchQuery.trim()) {
      return this.purchaseOrders.purchaseOrderItems;
    }
    const query = this.searchQuery.toLowerCase();
    return this.purchaseOrders.purchaseOrderItems.filter((item) =>
      item.productName.toLowerCase().includes(query)
    );
  }

  updatePagination() {
    const items = this.filteredItems;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;

    // Reset to page 1 if current page exceeds total pages
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = items.slice(startIndex, endIndex);
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openProductsForm() {
    if (!this.selectedSupplier || !this.selectedSupplier.products?.length) {
      this.selectSupplier = true;
      return;
    }
    this.openProductForm = true;
  }

  closeProductsForm() {
    this.openProductForm = false;
  }

  closeAddItemMessage() {
    this.addItem = false;
  }

  onProductsSelected(products: Product[]) {
    const items: PurchaseOrderItem[] = products.map((p) => {
      const isBoxOrder = p.poUnitType === 'Box';
      const initialQuantity = 1;

      return {
        id: 0,
        productId: p.productID,
        productName: p.productName,
        // Set quantities based on unit type
        boxQuantityOrdered: isBoxOrder ? initialQuantity : 0,
        boxQuantityReceived: 0,
        sheetQuantityOrdered: 0,
        sheetQuantityReceived: 0,
        pieceQuantityOrdered: isBoxOrder ? 0 : initialQuantity,
        pieceQuantityReceived: 0,
        boxPrice: p.boxPrice,
        sheetPrice: p.sheetPrice,
        piecePrice: p.piecePrice,
        // Calculate total based on unit type
        totalCost: isBoxOrder
          ? p.costBoxPrice * initialQuantity
          : p.costPiecePrice * initialQuantity,
        batchNumber: '',
        manufacturingDate: new Date(),
        expirationDate: new Date(),
        category: p.categoryName,
        classification: p.classificationName,
        type: p.typeName,
        unitMeasurement: p.unitMeasurement,
        poUnitType: p.poUnitType,
        quantityOrdered: initialQuantity,
        quantityReceived: 0,
        damagedPiecesCount: 0,
        expiredPiecesCount: 0,
        defectPiecesCount: 0,
      };
    });

    this.purchaseOrders.purchaseOrderItems.push(...items);
    this.updatePagination();
    this.computeTotalAmount();
    this.openProductForm = false;
  }

  recalculateQuantitiesAndTotals(item: PurchaseOrderItem) {
    const product = this.selectedSupplier.products.find(
      (p) => p.productID === item.productId
    );

    if (!product) return;

    // Reset
    item.boxQuantityOrdered = 0;
    item.sheetQuantityOrdered = 0;
    item.pieceQuantityOrdered = 0;

    if (product.poUnitType == 'Box') {
      item.boxQuantityOrdered = item.quantityOrdered;
      item.totalCost = item.boxPrice * item.quantityOrdered;
    } else if (product.poUnitType == 'Pieces') {
      item.pieceQuantityOrdered = item.quantityOrdered;
      item.totalCost = item.piecePrice * item.quantityOrdered;
    }
    this.computeTotalAmount();
  }

  getDisplayUnitPrice(request: PurchaseOrderItem) {
    return request.poUnitType == 'Box' ? request.boxPrice : request.piecePrice;
  }

  computeTotalAmount() {
    this.purchaseOrders.totalAmount =
      this.purchaseOrders.purchaseOrderItems.reduce(
        (sum, item) => sum + item.totalCost,
        0
      );
  }

  removeItemPaginated(item: PurchaseOrderItem) {
    const index = this.purchaseOrders.purchaseOrderItems.findIndex(
      (i) => i === item
    );
    if (index !== -1) {
      this.purchaseOrders.purchaseOrderItems.splice(index, 1);
      this.updatePagination();
      this.computeTotalAmount();
    }
  }

  createPurchaseOrder() {
    if (this.purchaseOrders.purchaseOrderItems.length === 0) {
      this.addItem = true;
      return;
    }

    if (!this.selectedSupplier.supplierName) {
      alert('Please select a supplier.');
      return;
    }

    const requestPayload: CreatePurchaseOrder = {
      purchaseOrderNumber: this.purchaseOrderNumber,
      orderDate: new Date().toISOString(),
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
          totalCost: item.totalCost,
          manufacturingDate: new Date(item.manufacturingDate).toISOString(),
          expirationDate: new Date(item.expirationDate).toISOString(),
          poUnitType: item.poUnitType,
          quantityReceived: 0
        })
      ),
    };

    this.postService.createPurchaseOrderAsync(requestPayload).subscribe({
      next: () => {
        this.showSuccessAlert = true;
        this.generateNewPurchaseOrderNumber();
        this.clearFields();
      },
      error: (err) => {
        console.error(err.error);
      },
    });
  }

  clearFields() {
    this.purchaseOrders = new PurchaseOrder();
    this.purchaseOrders.purchaseOrderItems = [];
    this.searchQuery = '';
    this.currentPage = 1;
    this.updatePagination();
  }

  getAllSuppliers() {
    this.getService.getAllSuppliersAsync().subscribe({
      next: (res) => (this.suppliers = res),
      error: (err) => console.error(err.error),
    });
  }

  getLoggedInUser() {
    const token = localStorage.getItem(LogInToken);
    if (!token) return;

    const decoded: any = jwtDecode(token);
    const employeeId = parseInt(decoded.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (res) => (this.employee = res),
      error: () => console.error('Logged-in user not found.'),
    });
  }

  generateNewPurchaseOrderNumber() {
    this.postService.generateNewPurchaseOrderNumberAsync().subscribe({
      next: (res) => {
        this.purchaseOrderNumber = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
  }

  closeSelectSupplierForm() {
    this.selectSupplier = false;
  }
}
