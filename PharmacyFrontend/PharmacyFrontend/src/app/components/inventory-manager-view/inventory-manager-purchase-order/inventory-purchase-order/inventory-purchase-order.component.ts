import { CommonModule } from '@angular/common';
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
import { Inventory } from '../../../../models/inventory-module-models/inventory';

@Component({
  selector: 'app-inventory-purchase-order',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    PurchaseOrderProductsComponent,
    CurrencyPipe,
  ],
  templateUrl: './inventory-purchase-order.component.html',
  styleUrl: './inventory-purchase-order.component.scss',
})
export class InventoryPurchaseOrderComponent implements OnInit {
  // Core data
  orderItems: PurchaseOrderItem[] = [];

  // Form data
  suppliers: Supplier[] = [];
  employee: Employee = new Employee();
  selectedSupplier: Supplier | null = null;
  purchaseOrderNumber: string = '';
  deliveryDate: Date = new Date();
  notes: string = '';
  totalAmount: number = 0;

  // Inventory alerts
  inventoriesOutOfStock: Inventory[] = [];
  inventoriesExpired: Inventory[] = [];
  showInventoryAlerts: boolean = true;

  // UI state
  openProductForm: boolean = false;
  showSuccessAlert: boolean = false;
  selectSupplier: boolean = false;
  addItem: boolean = false;

  // Pagination and search
  searchQuery: string = '';
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getAllSuppliers();
    this.getLoggedInUser();
    this.generateNewPurchaseOrderNumber();
    this.getAllInventoriesOutOfStockAsync();
    this.getAllExpiredInventories();
  }

  // Computed properties
  get filteredItems(): PurchaseOrderItem[] {
    if (!this.searchQuery.trim()) {
      return this.orderItems;
    }
    return this.orderItems.filter(item =>
      item.productName.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  get displayItems(): PurchaseOrderItem[] {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.filteredItems.slice(start, end);
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredItems.length / this.itemsPerPage));
  }

  // Modal controls
  openProductsForm(): void {
    if (!this.selectedSupplier) {
      this.selectSupplier = true;
      return;
    }
    if (!this.selectedSupplier.products?.length) {
      alert('The selected supplier has no products available.');
      return;
    }
    this.openProductForm = true;
  }

  closeProductsForm(): void {
    this.openProductForm = false;
  }

  closeAddItemMessage(): void {
    this.addItem = false;
  }

  dismissInventoryAlerts(): void {
    this.showInventoryAlerts = false;
  }

  closeSuccessAlert(): void {
    this.showSuccessAlert = false;
  }

  closeSelectSupplierForm(): void {
    this.selectSupplier = false;
  }

  // Add products
  onProductsSelected(products: Product[]): void {
    if (!products?.length) {
      return;
    }

    // Filter duplicates
    const existingIds = new Set(this.orderItems.map(i => i.productId));
    const newProducts = products.filter(p => !existingIds.has(p.productID));

    if (!newProducts.length) {
      alert('All selected products have already been added.');
      this.closeProductsForm();
      return;
    }

    // Create items
    newProducts.forEach(product => {
      const isBox = product.poUnitType === 'Box';
      const qty = 1;

      const item: PurchaseOrderItem = {
        id: 0,
        productId: product.productID,
        productName: product.productName,
        boxQuantityOrdered: isBox ? qty : 0,
        boxQuantityReceived: 0,
        sheetQuantityOrdered: 0,
        sheetQuantityReceived: 0,
        pieceQuantityOrdered: isBox ? 0 : qty,
        pieceQuantityReceived: 0,
        boxPrice: product.boxPrice || 0,
        sheetPrice: product.sheetPrice || 0,
        piecePrice: product.piecePrice || 0,
        totalCost: isBox
          ? (product.costBoxPrice || product.boxPrice || 0) * qty
          : (product.costPiecePrice || product.piecePrice || 0) * qty,
        batchNumber: '',
        manufacturingDate: new Date(),
        expirationDate: new Date(),
        category: product.categoryName || '',
        classification: product.classificationName || '',
        type: product.typeName || '',
        unitMeasurement: product.unitMeasurement || '',
        poUnitType: product.poUnitType || 'Pieces',
        quantityOrdered: qty,
        quantityReceived: 0,
        damagedPiecesCount: 0,
        expiredPiecesCount: 0,
        defectPiecesCount: 0,
      };

      this.orderItems.push(item);
    });

    // Reset selection
    products.forEach(p => p.selected = false);

    // Update totals
    this.updateTotal();

    // Close modal
    this.closeProductsForm();

    // Reset page if needed
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
  }

  // Remove item
  removeItem(item: PurchaseOrderItem): void {
    const index = this.orderItems.indexOf(item);
    if (index > -1) {
      this.orderItems.splice(index, 1);
      this.updateTotal();

      if (this.currentPage > this.totalPages) {
        this.currentPage = Math.max(1, this.totalPages);
      }
    }
  }

  // Update quantity
  onQuantityChange(item: PurchaseOrderItem): void {
    if (!this.selectedSupplier?.products) return;

    const product = this.selectedSupplier.products.find(p => p.productID === item.productId);
    if (!product) return;

    if (product.poUnitType === 'Box') {
      item.boxQuantityOrdered = item.quantityOrdered;
      item.pieceQuantityOrdered = 0;
      item.totalCost = item.boxPrice * item.quantityOrdered;
    } else {
      item.boxQuantityOrdered = 0;
      item.pieceQuantityOrdered = item.quantityOrdered;
      item.totalCost = item.piecePrice * item.quantityOrdered;
    }

    this.updateTotal();
  }

  getUnitPrice(item: PurchaseOrderItem): number {
    return item.poUnitType === 'Box' ? item.boxPrice : item.piecePrice;
  }

  updateTotal(): void {
    this.totalAmount = this.orderItems.reduce((sum, item) => sum + (item.totalCost || 0), 0);
  }

  // Search
  onSearchChange(): void {
    this.currentPage = 1;
  }

  // Pagination
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  // Create PO
  createPurchaseOrder(): void {
    if (!this.orderItems.length) {
      this.addItem = true;
      return;
    }

    if (!this.selectedSupplier) {
      alert('Please select a supplier.');
      return;
    }

    const payload: CreatePurchaseOrder = {
      purchaseOrderNumber: this.purchaseOrderNumber,
      orderDate: new Date().toISOString(),
      deliveryDate: new Date(this.deliveryDate).toISOString(),
      employeeLastName: this.employee.lastName,
      supplierName: this.selectedSupplier.supplierName,
      notes: this.notes,
      purchaseOrderItems: this.orderItems.map(item => ({
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
      }))
    };

    this.postService.createPurchaseOrderAsync(payload).subscribe({
      next: () => {
        this.showSuccessAlert = true;
        this.generateNewPurchaseOrderNumber();
        this.clearForm();
      },
      error: (err) => {
        console.error('Error:', err);
        alert('Failed to create purchase order.');
      }
    });
  }

  clearForm(): void {
    this.orderItems = [];
    this.selectedSupplier = null;
    this.notes = '';
    this.searchQuery = '';
    this.currentPage = 1;
    this.totalAmount = 0;
  }

  // API calls
  getAllSuppliers(): void {
    this.getService.getAllSuppliersAsync().subscribe({
      next: (res) => this.suppliers = res,
      error: (err) => console.error('Error loading suppliers:', err)
    });
  }

  getLoggedInUser(): void {
    const token = localStorage.getItem(LogInToken);
    if (!token) return;

    try {
      const decoded: any = jwtDecode(token);
      const employeeId = parseInt(decoded.EmployeeId);

      this.getService.GetEmployeeById(employeeId).subscribe({
        next: (res) => this.employee = res,
        error: () => console.error('Error loading user')
      });
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }

  generateNewPurchaseOrderNumber(): void {
    this.postService.generateNewPurchaseOrderNumberAsync().subscribe({
      next: (res) => this.purchaseOrderNumber = res,
      error: (err) => console.error('Error generating PO number:', err)
    });
  }

  getAllInventoriesOutOfStockAsync(): void {
    this.getService.getInventoriesOutOfStockAsync().subscribe({
      next: res => this.inventoriesOutOfStock = res,
      error: err => console.error('Error loading out of stock:', err)
    });
  }

  getAllExpiredInventories(): void {
    this.getService.getExpiredInventoriesAsync().subscribe({
      next: res => this.inventoriesExpired = res,
      error: err => console.error('Error loading expired:', err)
    });
  }
}
