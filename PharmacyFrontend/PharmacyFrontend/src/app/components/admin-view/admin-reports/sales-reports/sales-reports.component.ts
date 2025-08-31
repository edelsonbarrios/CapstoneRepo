import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgForOf } from "../../../../../../node_modules/@angular/common";

@Component({
  selector: 'app-sales-reports',
  standalone: true,
  imports: [FormsModule, NgFor],
  templateUrl: './sales-reports.component.html',
  styleUrl: './sales-reports.component.scss'
})
export class SalesReportsComponent implements OnInit{
  months: string[] = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  selectedMonth : string = '';

  constructor(

  ) {}

  ngOnInit(): void {
      
  }
}
