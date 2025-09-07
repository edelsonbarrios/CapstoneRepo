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
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}
