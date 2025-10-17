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
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';

@Component({
  selector: 'app-manage-employees',
  standalone: true,
  imports: [
    NgForOf, 
    ViewEmployeeComponent, 
    NgIf, 
    CreateEmployeeComponent, 
    FormsModule,
    ArchiveConfirmationComponent
  ],
  templateUrl: './manage-employees.component.html',
  styleUrl: './manage-employees.component.scss'
})
export class ManageEmployeesComponent implements OnInit{
  employees: Employee[] = []
  paginatedEmployees: Employee[] = [];
  viewEmployee ?: Employee | null;
  newEmployee : any;
  lastNameSearch : string = '';
  loggedInEmployeeId: number = 0;
  showSuccessAlert: boolean = false;
  showArchiveForm: number = 0;
  employeeToArchive: Employee = new Employee();
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 0;

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
        this.updatePagination();
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
        this.updatePagination();
        this.lastNameSearch = '';
      },
      error: err => {
        console.log(`${err.error}`);
        this.getAllEmployees();
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.employees.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedEmployees = this.employees.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveEmployee() {
    this.getService.GetEmployeeById(this.showArchiveForm).subscribe({
      next: res => {
        this.employeeToArchive = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    const requestPayload : ArchiveEmployeeRequest = {
      employeeId : this.employeeToArchive.id,
      actionPerformed : '',
      performedBy : this.loggedInEmployeeId,
      performedAt : new Date(this.employeeToArchive.birthDate).toISOString()
    };

    this.updateService.ArchiveEmployeeAsync(requestPayload).subscribe({
      next: mes => {
        this.showArchiveForm = 0;
        this.getAllEmployees();
      },
      error: err => {
        console.log(`${err.error}`);
        this.currentPage = 1;
        this.getAllEmployees();
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.currentPage = 1;
    this.getAllEmployees();
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