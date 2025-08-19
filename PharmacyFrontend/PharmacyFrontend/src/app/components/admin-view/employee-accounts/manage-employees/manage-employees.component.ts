import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { Employee } from '../../../../models/employee';
import { NgFor, NgForOf, NgIf } from "../../../../../../node_modules/@angular/common";
import { ViewEmployeeComponent } from '../view-employee/view-employee.component';

@Component({
  selector: 'app-manage-employees',
  standalone: true,
  imports: [NgForOf, ViewEmployeeComponent, NgIf],
  templateUrl: './manage-employees.component.html',
  styleUrl: './manage-employees.component.scss'
})
export class ManageEmployeesComponent implements OnInit{
  employees: Employee[] = []
  viewEmployee ?: Employee | null;
  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllEmployees();
  }

  getAllEmployees() {
    this.getService.GetAllEmployees().subscribe({
      next: employees => {
        this.employees = employees;
        console.log('Employees successfully selected.');
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  openViewEmployeeForm(employee : Employee) {
    this.viewEmployee = employee;
  }

  closeViewEmployeeForm() {
    this.viewEmployee = null;
  }
}
