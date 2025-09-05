import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { Employee } from '../../../../models/employee-models/employee';
import { NgFor, NgForOf, NgIf } from "../../../../../../node_modules/@angular/common";
import { ViewEmployeeComponent } from '../view-employee/view-employee.component';
import { CreateEmployeeComponent } from '../create-employee/create-employee.component';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { ArchiveEmployeeRequest } from '../../../../models/employee-models/archiveEmployeeRequest';
import { LogInToken } from '../../../shared/token';
import { jwtDecode } from 'jwt-decode';

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
  loggedInEmployeeId: number = 0;

  constructor(
    private getService : GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllEmployees();
    this.getLoggedInEmployee();
  }

  getAllEmployees() {
    this.getService.GetDisplayEmployees().subscribe({
      next: employees => {
        this.employees = employees;
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

  archiveEmployee(employee: Employee) {
    if(!employee) return ;
    const requestPayload : ArchiveEmployeeRequest = {
      employeeId : employee.id,
      actionPerformed : '',
      performedBy : this.loggedInEmployeeId,
      performedAt : new Date(employee.birthDate).toISOString()
    };

    this.updateService.ArchiveEmployeeAsync(requestPayload).subscribe({
      next: mes => {
        alert(`${mes.message}`);
        this.getAllEmployees();
      },
      error: err => {
        alert(`${err.error}`);
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

  getLoggedInEmployee() {
      var token = localStorage.getItem(LogInToken);
      if(!token) {
        console.log('Token does not exist.');
        return;
      }
  
      var employee : any = jwtDecode(token);
      console.log("Decoded token:", employee);
      var employeeId = parseInt(employee.EmployeeId);

      this.loggedInEmployeeId = employeeId;
    }
}
