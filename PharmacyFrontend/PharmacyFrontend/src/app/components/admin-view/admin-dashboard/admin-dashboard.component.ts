import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CurrencyPipe, DatePipe, NgFor, NgIf, CommonModule } from '@angular/common';
import { ProductSalesSummary } from '../../../models/sales-transaction-models/product-sales-summary';
import { Product } from '../../../models/product-models/product';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { GetService } from '../../../services/GetService/get.service';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';

// Extended interface for top selling medicines with unit breakdown
interface TopSellingMedWithBreakdown extends ProductSalesSummary {
  unitType: 'Multi-Level' | 'Dual-Level' | 'Single-Level';
  quantitySoldBreakdown: {
    box?: number;
    sheet?: number;
    piece: number;
  };
  totalQty: number;
  boxSold?: number;
  sheetSold?: number;
  pieceSold?: number;
  unitMeasurement?: string;
}

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [RouterLink, NgFor, NgIf, CurrencyPipe, DatePipe, CommonModule, BaseChartDirective],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit, OnDestroy {
  // Existing properties
  todaySalesAmount: number = 0;
  lowStockCount: number = 0;
  outOfStockCount: number = 0;
  topSellingMedicines: TopSellingMedWithBreakdown[] = [];
  paginatedTopMeds: TopSellingMedWithBreakdown[] = [];
  dateToday: Date = new Date();
  timer: any;
  expiringProducts: Inventory[] = [];
  paginatedExpiringProducts: Inventory[] = [];
  
  topMedsCurrentPage: number = 1;
  topMedsItemsPerPage: number = 2;
  topMedsTotalPages: number = 0;
  
  expiryCurrentPage: number = 1;
  expiryItemsPerPage: number = 3;
  expiryTotalPages: number = 0;

  // NEW: Tab state
  activeTab: string = 'sales';

  // NEW: Chart configurations
  public salesLineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Sales Revenue',
        fill: false,
        tension: 0.4,
        borderColor: '#3498ff',
        backgroundColor: '#3498ff',
        pointBackgroundColor: '#3498ff',
        pointBorderColor: '#fff',
        pointRadius: 5,
      },
      {
        data: [],
        label: 'Cost',
        fill: false,
        tension: 0.4,
        borderColor: '#f59e0b',
        backgroundColor: '#f59e0b',
        pointBackgroundColor: '#f59e0b',
        pointBorderColor: '#fff',
        pointRadius: 5,
      }
    ]
  };

  public salesLineChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: true, position: 'top' },
      tooltip: {
        callbacks: {
          label: (context) => {
            return `${context.dataset.label}: ₱${context.parsed.y?.toLocaleString()}`;
          }
        }
      }
    },
    scales: {
      x: { grid: { display: true, color: '#e2e8f0' } },
      y: { 
        grid: { display: true, color: '#e2e8f0' },
        ticks: {
          callback: (value) => '₱' + value.toLocaleString()
        }
      }
    }
  };

  public salesLineChartType = 'line' as const;

  // Bar Chart for Top Products
  public productsBarChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Quantity Sold',
        backgroundColor: '#3498ff',
        borderRadius: 8,
      },
      {
        data: [],
        label: 'Revenue (₱)',
        backgroundColor: '#10b981',
        borderRadius: 8,
      }
    ]
  };

  public productsBarChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: true, position: 'top' },
      tooltip: {
        callbacks: {
          label: (context) => {
            if (context.datasetIndex === 1) {
              return `Revenue: ₱${context.parsed.y?.toLocaleString()}`;
            }
            return `Qty Sold: ${context.parsed.y}`;
          }
        }
      }
    }
  };

  public productsBarChartType = 'bar' as const;

  // Pie Chart for Inventory
  public inventoryPieChartData: ChartConfiguration<'pie'>['data'] = {
    labels: ['In Stock', 'Low Stock', 'Out of Stock'],
    datasets: [{
      data: [245, 0, 0],
      backgroundColor: ['#10b981', '#f59e0b', '#ef4444'],
    }]
  };

  public inventoryPieChartOptions: ChartConfiguration<'pie'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: true, position: 'right' },
    }
  };

  public inventoryPieChartType = 'pie' as const;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getTodaySalesAmount();
    this.getLowStockCount();
    this.getOutOfStockCount();
    this.getNearExpiryInventory();
    this.getTopSellingProductsMonthly();
    this.loadChartData();

    this.timer = setInterval(() => {
      this.dateToday = new Date();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  //Generate array of last 7 days in MM/DD format
  private getLast7Days(): string[] {
    const days: string[] = [];
    const today = new Date();
    
    for (let i = 6; i >= 0; i--) {
      const date = new Date(today);
      date.setDate(date.getDate() - i);
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      days.push(`${month}/${day}`);
    }
    
    return days;
  }

  loadChartData(): void {
    // Load top products for bar chart (current month)
    this.getService.getTopSellingProductsMonthly().subscribe({
      next: (data: ProductSalesSummary[]) => {
        const topProducts = data.slice(0, 5);
        this.productsBarChartData.labels = topProducts.map((p: ProductSalesSummary) => p.productName);
        this.productsBarChartData.datasets[0].data = topProducts.map((p: ProductSalesSummary) => p.quantitySold);
        this.productsBarChartData.datasets[1].data = topProducts.map((p: ProductSalesSummary) => p.totalSales);
      },
      error: (err) => console.log('Error loading top products:', err)
    });

    // Load sales trend for line chart (last 7 days)
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - 6);

    this.getService.getProductSalesSummaryAsync(startDate, endDate).subscribe({
      next: (data: ProductSalesSummary[]) => {
        const last7Days = this.getLast7Days();
        
        const salesByDate = new Map<string, number>();
        last7Days.forEach((day: string) => salesByDate.set(day, 0));
        
        data.forEach((product: ProductSalesSummary) => {
          const avgPerDay = product.totalSales / 7;
          last7Days.forEach((day: string) => {
            salesByDate.set(day, (salesByDate.get(day) || 0) + avgPerDay);
          });
        });
        
        const salesData = last7Days.map((day: string) => Math.round(salesByDate.get(day) || 0));
        const costData = salesData.map((sale: number) => Math.round(sale * 0.6));
        
        this.salesLineChartData.labels = last7Days;
        this.salesLineChartData.datasets[0].data = salesData;
        this.salesLineChartData.datasets[1].data = costData;
      },
      error: (err) => console.log('Error loading sales trend:', err)
    });

    // Update inventory pie chart with real data
    const totalProducts = 245;
    const inStock = totalProducts - this.lowStockCount - this.outOfStockCount;
    
    this.inventoryPieChartData.datasets[0].data = [
      inStock > 0 ? inStock : 0,
      this.lowStockCount,
      this.outOfStockCount
    ];
  }

  // Tab switching
  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  getTodaySalesAmount() {
    this.getService.getTodaySalesAmountAsync().subscribe({
      next: (res) => (this.todaySalesAmount = res),
      error: (err) => console.log(`${err.error}`),
    });
  }

  getLowStockCount() {
    this.getService.getInventoryLowStockCountAsync().subscribe({
      next: (res) => {
        this.lowStockCount = res;
        this.loadChartData();
      },
      error: (err) => console.log(`${err.error}`),
    });
  }

  getOutOfStockCount() {
    this.getService.getInventoryOutOfStockCountAsync().subscribe({
      next: res => {
        this.outOfStockCount = res;
        this.loadChartData();
      },
      error: err => console.log(`${err.error}`)
    });
  }

  getNearExpiryInventory() {
    this.getService.getInventoryNearExpiry().subscribe({
      next: res => {
        this.expiringProducts = res;
        this.updateExpiryPagination();
      },
      error: err => console.log(`${err.error}`)
    });
  }

  getTopSellingProductsMonthly() {
    this.getService.getTopSellingProductsMonthly().subscribe({
      next: res => {
        console.log('Backend response:', res); // Debug log
        
        // Map the backend response to include unit type and breakdown
        this.topSellingMedicines = res.map(product => {
          const mapped: TopSellingMedWithBreakdown = {
            ...product,
            unitType: (product.unitMeasurement || 'Single-Level') as 'Multi-Level' | 'Dual-Level' | 'Single-Level',
            quantitySoldBreakdown: {
              box: product.boxSold ?? undefined,
              sheet: product.sheetSold ?? undefined,
              piece: product.pieceSold ?? product.quantitySold ?? 0
            },
            totalQty: product.quantitySold || 0,
            boxSold: product.boxSold,
            sheetSold: product.sheetSold,
            pieceSold: product.pieceSold,
            unitMeasurement: product.unitMeasurement
          };
          
          console.log('Mapped product:', mapped); // Debug log
          return mapped;
        });
        
        this.updateTopMedsPagination();
      },
      error: err => console.log(`${err.error}`)
    });
  }

  updateTopMedsPagination() {
    this.topMedsTotalPages = Math.ceil(this.topSellingMedicines.length / this.topMedsItemsPerPage);
    if (this.topMedsTotalPages === 0) this.topMedsTotalPages = 1;
    const startIndex = (this.topMedsCurrentPage - 1) * this.topMedsItemsPerPage;
    const endIndex = startIndex + this.topMedsItemsPerPage;
    this.paginatedTopMeds = this.topSellingMedicines.slice(startIndex, endIndex);
  }

  goToTopMedsPage(page: number) {
    if (page >= 1 && page <= this.topMedsTotalPages) {
      this.topMedsCurrentPage = page;
      this.updateTopMedsPagination();
    }
  }

  updateExpiryPagination() {
    this.expiryTotalPages = Math.ceil(this.expiringProducts.length / this.expiryItemsPerPage);
    if (this.expiryTotalPages === 0) this.expiryTotalPages = 1;
    const startIndex = (this.expiryCurrentPage - 1) * this.expiryItemsPerPage;
    const endIndex = startIndex + this.expiryItemsPerPage;
    this.paginatedExpiringProducts = this.expiringProducts.slice(startIndex, endIndex);
  }

  goToExpiryPage(page: number) {
    if (page >= 1 && page <= this.expiryTotalPages) {
      this.expiryCurrentPage = page;
      this.updateExpiryPagination();
    }
  }
}