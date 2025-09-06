import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee-models/employee';
import { FormsModule } from '@angular/forms';
import { NgIf, DatePipe} from '@angular/common';
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
  @Input() employee : Employee = new Employee();
  @Output() closeForm = new EventEmitter<void>();
  isEditMode : boolean = false;

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
    const requestPayload: UpdateEmployeeRequest = {
      id: employee.id,
      employeeCode: employee.employeeCode,
      lastName: employee.lastName,
      firstName: employee.firstName,
      middleName: employee.middleName,
      age: employee.age,
      birthDate: new Date(employee.birthDate).toISOString(),
      civilStatus: employee.civilStatus,
      gender: employee.gender,
      email: employee.email,
      address: employee.address,
      phoneNumber: employee.phoneNumber,
      rolePosition: employee.rolePosition
    };

    this.updateService.UpdateEmployeeAsync(employee).subscribe({
      next: res => {
        alert(`${res.message}`);
        this.isEditMode = false;
      },
      error: err => {
        alert(`${err.error.message}`);
      }
    });
  }
}
