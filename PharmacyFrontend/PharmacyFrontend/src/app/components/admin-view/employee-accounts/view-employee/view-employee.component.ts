import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee-models/employee';
import { FormsModule } from '@angular/forms';
import { NgIf, DatePipe } from '@angular/common';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { UpdateEmployeeRequest } from '../../../../models/employee-models/updateEmployeeRequest';

@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [FormsModule, NgIf, DatePipe],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.scss'
})
export class ViewEmployeeComponent {
  @Input() employee: Employee = new Employee();
  @Output() closeForm = new EventEmitter<void>();
  isEditMode: boolean = false;
  showSuccessAlert: boolean = false;

  // Error properties
  employeeCodeError: string = '';
  lastNameError: string = '';
  firstNameError: string = '';
  genderError: string = '';
  birthDateError: string = '';
  addressError: string = '';

  constructor(
    private updateService: UpdateService
  ) {}

  closeViewForm() {
    this.closeForm.emit();
  }

  toggleViewMode() {
    this.isEditMode = !this.isEditMode;
    // Clear errors when entering edit mode
    this.clearErrors();
  }

  toggleEditMode(employee: Employee) {
    // Clear previous errors
    this.clearErrors();

    let hasErrors = false;

    // Validate required fields
    if (!employee.employeeCode || employee.employeeCode.trim() === '') {
      this.employeeCodeError = 'Employee code is required';
      hasErrors = true;
    }

    if (!employee.lastName || employee.lastName.trim() === '') {
      this.lastNameError = 'Last name is required';
      hasErrors = true;
    }

    if (!employee.firstName || employee.firstName.trim() === '') {
      this.firstNameError = 'First name is required';
      hasErrors = true;
    }

    if (!employee.gender || employee.gender.trim() === '') {
      this.genderError = 'Gender is required';
      hasErrors = true;
    }

    if (!employee.address || employee.address.trim() === '') {
      this.addressError = 'Address is required';
      hasErrors = true;
    }

    // Validate birthdate age range (21-60 years old)
    if (employee.birthDate) {
      const birthDate = new Date(employee.birthDate);
      const today = new Date();
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDiff = today.getMonth() - birthDate.getMonth();
      
      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }

      if (age < 21 || age > 60) {
        this.birthDateError = 'Employee must be between 21 and 60 years old';
        hasErrors = true;
      }
    }

    // If there are validation errors, don't proceed
    if (hasErrors) {
      return;
    }

    // Handle birthDate conversion properly
    let birthDateISO: string;
    
    if (typeof employee.birthDate === 'string') {
      // If it's already a string (from date input), convert to ISO
      const dateObj = new Date(employee.birthDate);
      birthDateISO = dateObj.toISOString();
    } else {
      // If it's a Date object
      birthDateISO = new Date(employee.birthDate).toISOString();
    }

    const requestPayload: UpdateEmployeeRequest = {
      id: employee.id,
      employeeCode: employee.employeeCode,
      lastName: employee.lastName,
      firstName: employee.firstName,
      middleName: employee.middleName,
      age: employee.age,
      birthDate: birthDateISO,
      civilStatus: employee.civilStatus,
      gender: employee.gender,
      email: employee.email,
      address: employee.address,
      phoneNumber: employee.phoneNumber,
      rolePosition: employee.rolePosition
    };

    // Debug log to check the payload
    console.log('Request Payload:', requestPayload);
    console.log('Role Position being sent:', requestPayload.rolePosition);

    // Pass the requestPayload
    this.updateService.UpdateEmployeeAsync(requestPayload).subscribe({
      next: res => {
        console.log('Update successful:', res);
        this.showSuccessAlert = true;
      },
      error: err => {
        console.error('Update error:', err);
        alert(`${err.error?.message || 'Failed to update employee'}`);
      }
    });
  }

  clearErrors() {
    this.employeeCodeError = '';
    this.lastNameError = '';
    this.firstNameError = '';
    this.genderError = '';
    this.birthDateError = '';
    this.addressError = '';
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.isEditMode = false;
    this.clearErrors();
    this.closeForm.emit();
  }
}