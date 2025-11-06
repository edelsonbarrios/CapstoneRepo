import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  loginVerify: boolean = false;
  
  // Form fields
  boxQuantityChanged: number = 0;
  sheetQuantityChanged: number = 0;
  pieceQuantityChanged: number = 0;
  notes: string = '';
  refNumber: string = '';
  movementType: string = '';
  movementTypes: string[] = ['Damaged', 'Expired', 'Count Correction'];
  
  // UI states
  showSuccessAlert: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  constructor(
    private postService: PostService,
    private getService: GetService
  ) {}

  ngOnInit(): void {
    this.getLoggedInUser();
  }

  // This is the entry point - called when user clicks "Save Adjustment"
  createNewStockMovement() {
    // Validation
    if (!this.validateForm()) {
      return;
    }

    if (!this.stockAdjust) {
      this.errorMessage = 'No inventory selected';
      return;
    }

    // Show login verification BEFORE processing
    this.openLoginVerifyForm();
  }

  // This gets called AFTER successful verification
  processStockAdjustment() {
    this.isSubmitting = true;
    this.errorMessage = '';

    const requestPayload: StockAdjustment = {
      InventoryID: this.stockAdjust.inventoryID,
      MovementType: this.movementType,
      BoxQuantityAdjustment: this.boxQuantityChanged,
      SheetQuantityAdjustment: this.sheetQuantityChanged,
      PieceQuantityAdjustment: this.pieceQuantityChanged,
      ReferenceNumber: this.refNumber,
      Notes: this.notes,
      EmployeeID: this.employee.id
    };

    this.postService.adjustInventoryStockAsync(requestPayload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        this.showSuccessAlert = true;
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || err.error || 'Failed to adjust stock. Please try again.';
        console.error('Stock adjustment error:', err);
      }
    });
  }

  validateForm(): boolean {
    // Check if movement type is selected
    if (!this.movementType) {
      this.errorMessage = 'Please select a movement type';
      return false;
    }

    // Check if at least one quantity is adjusted
    const hasAdjustment = this.boxQuantityChanged !== 0 || 
                          this.sheetQuantityChanged !== 0 || 
                          this.pieceQuantityChanged !== 0;

    if (!hasAdjustment) {
      this.errorMessage = 'Please adjust at least one quantity (Box, Sheet, or Piece)';
      return false;
    }

    // Check if reference number is provided
    if (!this.refNumber || this.refNumber.trim() === '') {
      this.errorMessage = 'Please provide a reference number';
      return false;
    }

    this.errorMessage = '';
    return true;
  }

  getLoggedInUser() {
    const token = localStorage.getItem(LogInToken);
    if (!token) {
      console.error('No authentication token found');
      return;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      const employeeId = parseInt(decodedToken.EmployeeId);
      
      if (!employeeId) {
        console.error('Employee ID does not exist in token');
        return;
      }

      this.getService.GetEmployeeById(employeeId).subscribe({
        next: (res) => {
          this.employee = res;
        },
        error: (err) => {
          console.error('Failed to fetch logged-in user:', err);
        },
      });
    } catch (error) {
      console.error('Failed to decode token:', error);
    }
  }

  // Opens the login verification modal
  openLoginVerifyForm() {
    this.loginVerify = true;
  }

  // Closes verification and processes the adjustment
  closeVerificationForm() {
    this.loginVerify = false;
    // After successful verification, process the adjustment
    this.processStockAdjustment();
  }

  closeStockAdjustmentForm() {
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeStockAdjustmentForm();
  }

  clearForm() {
    this.boxQuantityChanged = 0;
    this.sheetQuantityChanged = 0;
    this.pieceQuantityChanged = 0;
    this.notes = '';
    this.refNumber = '';
    this.movementType = '';
    this.errorMessage = '';
  }
}