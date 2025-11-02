import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateStockMovement } from '../../../../models/inventory-module-models/createStockMovement';
import { PostService } from '../../../../services/PostService/post.service';
import { Inventory } from '../../../../models/inventory-module-models/inventory';
import { GetService } from '../../../../services/GetService/get.service';
import { Employee } from '../../../../models/employee-models/employee';
import { jwtDecode } from 'jwt-decode';
import { LogInToken } from '../../../shared/token';
import { FormsModule } from '@angular/forms';
import { NgClass, CommonModule } from "@angular/common";
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';
import { StockAdjustment } from '../../../../models/inventory-module-models/stock-adjustment';
import { LoginVerificationComponent } from '../login-verification/login-verification.component';

@Component({
  selector: 'app-stock-adjust',
  standalone: true,
  imports: [FormsModule, CommonModule, LoginVerificationComponent],
  templateUrl: './stock-adjust.component.html',
  styleUrl: './stock-adjust.component.scss',
})
export class StockAdjustComponent implements OnInit {
  @Input() stockAdjust: StockMovement = new StockMovement();
  @Output() closeForm = new EventEmitter<void>();
  employee: Employee = new Employee();
  loginVerify?: Inventory | null;
  boxQuantityChanged: number = 0;
  sheetQuantityChanged: number = 0;
  pieceQuantityChanged: number = 0;
  notes : string = '';
  refNumber: string = '';
  movementType: string = '';
  movementTypes : string[] = ['Damaged', 'Expired', 'Loss', 'Found', 'Count Correction', 'Transfer', 'Return'];
  showSuccessAlert: boolean = false;

  constructor(
    private postService: PostService,
    private getService: GetService
  ) {}

  ngOnInit(): void {
    this.getLoggedInUser();
  }

  createNewStockMovement() {
    if(!this.stockAdjust) return;
    const requestPayload : StockAdjustment = {
      inventoryID : this.stockAdjust.inventoryID,
      movementType : this.movementType,
      boxQuantityAdjustment: this.boxQuantityChanged,
      sheetQuantityAdjustment: this.sheetQuantityChanged,
      pieceQuantityAdjustment: this.pieceQuantityChanged,
      referenceNumber : this.refNumber,
      notes: this.notes,
      employeeID : this.employee.id
    }
    this.postService.adjustInventoryStockAsync(requestPayload).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
      }
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

  openLoginVerifyForm() {
    this.loginVerify = new Inventory();
  } 

  closeStockAdjustmentForm() {
    this.closeForm.emit();
    console.log(this.stockAdjust);
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeStockAdjustmentForm();
  }

  closeVerificationForm() {
    this.loginVerify = null;
  }
}
