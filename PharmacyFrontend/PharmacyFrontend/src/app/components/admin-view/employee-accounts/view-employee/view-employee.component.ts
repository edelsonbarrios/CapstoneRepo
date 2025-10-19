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

  constructor(
    private updateService: UpdateService
  ) {}

  closeViewForm() {
    this.closeForm.emit();
  }

  toggleViewMode() {
    this.isEditMode = !this.isEditMode;
  }

  toggleEditMode(employee: Employee) {
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

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.isEditMode = false;
    this.closeForm.emit();
  }
}