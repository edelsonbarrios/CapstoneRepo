import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateInventory } from '../../../../models/inventory-module-models/createInventory';
import { PostService } from '../../../../services/PostService/post.service';
import { GetService } from '../../../../services/GetService/get.service';
import { Product } from '../../../../models/product-models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Employee } from '../../../../models/employee-models/employee';
import { jwtDecode } from 'jwt-decode';
import { LogInToken } from '../../../shared/token';

@Component({
  selector: 'app-create-stock',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-stock.component.html',
  styleUrl: './create-stock.component.scss'
})
export class CreateStockComponent implements OnInit{
  @Input() createStock: CreateInventory = new CreateInventory;
  @Output() closeForm = new EventEmitter<void>();
  products : Product[] = [];
  employee: Employee = new Employee();
  employeeId: number = 0;
  selectedProduct : Product = new Product();
  showSuccessAlert: boolean = false;

  // Validation error properties
  productError: string = '';
  batchNumberError: string = '';
  manufacturingDateError: string = '';
  expirationDateError: string = '';
  costPriceError: string = '';

  constructor(
    private postService : PostService,
    private getService : GetService
  ) { }

  ngOnInit(): void {
    this.getAllProducts();
    this.getLoggedInEmployee();
  }

  onSelectProduct(request: Product) {
    this.selectedProduct = request;
    if (this.selectedProduct && this.selectedProduct.productName) {
      this.productError = '';
    }

    this.createStock.sheetsPerBox = this.selectedProduct.sheetsPerBox;
    this.createStock.piecesPerSheet = this.selectedProduct.piecesPerSheet;
    this.createStock.reorderLevel = this.selectedProduct.reorderLevel;
    this.createStock.boxPrice = this.selectedProduct.boxPrice;
    this.createStock.sheetPrice = this.selectedProduct.sheetPrice;
    this.createStock.piecePrice = this.selectedProduct.piecePrice;
  }

  createInventory() {
    this.productError = '';
    this.batchNumberError = '';
    this.manufacturingDateError = '';
    this.expirationDateError = '';
    this.costPriceError = '';

    let isValid = true;

    if (!this.selectedProduct || !this.selectedProduct.productName) {
      this.productError = 'Product is required';
      isValid = false;
    }

    if (!this.createStock.batchNumber || this.createStock.batchNumber.trim() === '') {
      this.batchNumberError = 'Batch number is required';
      isValid = false;
    }

    if (!this.createStock.manufacturingDate) {
      this.manufacturingDateError = 'Manufacturing date is required';
      isValid = false;
    } else {
      const mfgDate = new Date(this.createStock.manufacturingDate);
      const today = new Date();
      today.setHours(0, 0, 0, 0);

      const fiveYearsAgo = new Date(today);
      fiveYearsAgo.setFullYear(today.getFullYear() - 5);

      if (mfgDate > today) {
        this.manufacturingDateError = 'Invalid Date';
        isValid = false;
      } else if (mfgDate < fiveYearsAgo) {
        this.manufacturingDateError = 'Invalid Date';
        isValid = false;
      }
    }

    if (!this.createStock.expirationDate) {
      this.expirationDateError = 'Expiration date is required';
      isValid = false;
    } else {
      const expDate = new Date(this.createStock.expirationDate);
      const today = new Date();
      today.setHours(0, 0, 0, 0);

      if (expDate <= today) {
        this.expirationDateError = 'Invalid Date';
        isValid = false;
      }
    }

    if (!this.createStock.costPrice || this.createStock.costPrice <= 0) {
      this.costPriceError = 'Cost price is required';
      isValid = false;
    }

    if (!isValid) {
      return;
    }

    if(!this.createStock) return;
    if(!this.selectedProduct) return;
    const requestPayload : CreateInventory = {
      productName : this.selectedProduct.productName,
      batchNumber: this.createStock.batchNumber,
      manufacturingDate : new Date(this.createStock.manufacturingDate).toISOString(),
      expirationDate : new Date(this.createStock.expirationDate).toISOString(),
      boxQuantity : this.createStock.boxQuantity,
      sheetQuantity : this.createStock.sheetQuantity,
      pieceQuantity : this.createStock.pieceQuantity,
      sheetsPerBox : this.createStock.sheetsPerBox,
      piecesPerSheet: this.createStock.piecesPerSheet,
      reorderLevel : this.createStock.reorderLevel,
      costPrice : this.createStock.costPrice,
      boxPrice : this.createStock.boxPrice,
      sheetPrice : this.createStock.sheetPrice,
      piecePrice : this.createStock.piecePrice,
      referenceNumber : '',
      notes: '',
      employeeID: this.employee.id,
      piecePerBox: this.selectedProduct.piecePerBox
    }
    this.postService.createNewInventoryAsync(requestPayload).subscribe({
      next: res => {
        this.showSuccessAlert = true;
        console.log('✅ Inventory created:', res);
      },
      error: err => {
        console.log(`${err.error}`);
        console.log(requestPayload);
      }
    });
  }


  closeCreateInventoryForm() {
    this.productError = '';
    this.batchNumberError = '';
    this.manufacturingDateError = '';
    this.expirationDateError = '';
    this.costPriceError = '';
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateInventoryForm();
  }

  getAllProducts() {
    this.getService.getAllProductsAsync().subscribe({
      next: res => {
        this.products = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getLoggedInEmployee() {
    const token = localStorage.getItem(LogInToken);
    if (!token) {
      console.log(`Token does not exist.`);
      return;
    }

    const employee: any = jwtDecode(token);
    const employeeId = parseInt(employee.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }
}
