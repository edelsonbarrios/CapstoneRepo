import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { InventoryDashboard } from '../../../models/inventory-module-models/inventory-dashboard';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-inventory-manager-dashboard',
  standalone: true,
  imports: [FormsModule, CommonModule, DatePipe],
  templateUrl: './inventory-manager-dashboard.component.html',
  styleUrl: './inventory-manager-dashboard.component.scss'
})
export class InventoryManagerDashboardComponent implements OnInit{
  inventoryDashboard: InventoryDashboard = new InventoryDashboard();

  constructor(
    private getService: GetService
  ) {}

  ngOnInit(): void {
    this.getInventoryDashboardValues();
  }

  getInventoryDashboardValues() {
    this.getService.getInventoryDashboardValues().subscribe({
      next: res => {
        this.inventoryDashboard = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}
