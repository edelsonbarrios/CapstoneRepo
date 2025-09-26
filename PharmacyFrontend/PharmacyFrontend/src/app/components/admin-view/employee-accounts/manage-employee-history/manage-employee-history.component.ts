import { Component, OnInit } from '@angular/core';
import { EmployeeHistory } from '../../../../models/employee-history-models/employee-history';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgFor } from "../../../../../../node_modules/@angular/common";

@Component({
  selector: 'app-manage-employee-history',
  standalone: true,
  imports: [FormsModule, NgFor, DatePipe],
  templateUrl: './manage-employee-history.component.html',
  styleUrl: './manage-employee-history.component.scss'
})
export class ManageEmployeeHistoryComponent implements OnInit{
  employeeHistories : EmployeeHistory[] = [];
  paginatedEmployeeHistories: EmployeeHistory[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getDisplayEmployeeHistory();
  }

  getAllEmployeeHistories() {
    this.getService.getAllEmployeeHistoriesAsync().subscribe({
      next: eh => {
        this.employeeHistories = eh;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getDisplayEmployeeHistory() {
    this.getService.getDisplayEmployeeHistoriesAsync().subscribe({
      next: eh => {
        this.employeeHistories = eh;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.employeeHistories.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedEmployeeHistories = this.employeeHistories.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }
}