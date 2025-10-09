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
  selectedProduct : Product = new Product();

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

    this.createStock.sheetsPerBox = this.selectedProduct.sheetsPerBox;
    this.createStock.piecesPerSheet = this.selectedProduct.piecesPerSheet;
    this.createStock.reorderLevel = this.selectedProduct.reorderLevel;
    this.createStock.boxPrice = this.selectedProduct.boxPrice;
    this.createStock.sheetPrice = this.selectedProduct.sheetPrice;
    this.createStock.piecePrice = this.selectedProduct.piecePrice;
  }

  createInventory() {
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
      employeeID: this.employee.id
    }
    this.postService.createNewInventoryAsync(requestPayload).subscribe({
      next: res => {
        alert(`Inventory successfully created.`);
        this.closeCreateInventoryForm();
      },
      error: err => {
        console.log(`${err.error}`);
        console.log(requestPayload);
      }
    });
  }

  closeCreateInventoryForm() {
    this.closeForm.emit();
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
