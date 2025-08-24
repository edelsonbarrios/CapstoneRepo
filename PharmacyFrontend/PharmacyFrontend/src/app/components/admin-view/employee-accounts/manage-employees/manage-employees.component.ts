import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { Employee } from '../../../../models/employee';
import { NgFor, NgForOf, NgIf } from "../../../../../../node_modules/@angular/common";
import { ViewEmployeeComponent } from '../view-employee/view-employee.component';
import { CreateEmployeeComponent } from '../create-employee/create-employee.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-employees',
  standalone: true,
  imports: [NgForOf, ViewEmployeeComponent, NgIf, CreateEmployeeComponent, FormsModule],
  templateUrl: './manage-employees.component.html',
  styleUrl: './manage-employees.component.scss'
})
export class ManageEmployeesComponent implements OnInit{
  employees: Employee[] = []
  viewEmployee ?: Employee | null;
  newEmployee : any;
  lastNameSearch : string = '';

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllEmployees();
  }

  getAllEmployees() {
    this.getService.GetDisplayEmployees().subscribe({
      next: employees => {
        this.employees = employees;
        console.log('Employees successfully selected.');
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getEmployeesByLastName() {
    this.getService.GetEmployeesByLastName(this.lastNameSearch).subscribe({
      next: employees => {
        this.employees = employees;
        this.lastNameSearch = '';
      },
      error: err => {
        console.log(`${err.error}`);
        this.getAllEmployees();
      }
    });
  }

  openViewEmployeeForm(employee : Employee) {
    this.viewEmployee = employee;
  }

  closeViewEmployeeForm() {
    this.viewEmployee = null;
  }

  openCreateEmployeeForm() {
    this.newEmployee = new Employee();
  }

  closeCreateEmployeeForm() {
    this.newEmployee = null;
    this.getAllEmployees();
  }
}
