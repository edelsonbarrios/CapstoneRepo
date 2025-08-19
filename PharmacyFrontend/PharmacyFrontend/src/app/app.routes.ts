import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => {
      return import('./components/login-view/login-view.component').then(m => m.LoginViewComponent);
    }
  },
  {
    path: 'admin-main',
    loadComponent: () => {
      return import('./components//admin-view/admin-main/admin-main.component').then(m => m.AdminMainComponent);
    },
    children: [
      {
        path: 'admin-dashboard',
        loadComponent: () => {
          return import('./components/admin-view/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent);
        },
      },
      {
        path: 'admin-reports-main',
        loadComponent:() => {
          return import('./components/admin-view/admin-reports/admin-reports-main/admin-reports-main.component').then(m => m.AdminReportsMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import('./components/admin-view/admin-reports/sales-reports/sales-reports.component').then(m => m.SalesReportsComponent);
          }
          },
          {
            path: 'sales-reports',
            loadComponent: () => {
              return import('./components/admin-view/admin-reports/sales-reports/sales-reports.component').then(m => m.SalesReportsComponent);
          }
          },
          {
            path: 'inventory-reports',
            loadComponent: () => {
              return import('./components/admin-view/admin-reports/inventory-reports/inventory-reports.component').then(m => m.InventoryReportsComponent);
          }
          },
          {
            path: 'fast-moving-products',
            loadComponent: () => {
              return import('./components/admin-view/admin-reports/fast-moving-products/fast-moving-products.component').then(m => m.FastMovingProductsComponent);
          }
          }
        ]
      },
      {
        path: 'employee-accounts-main',
        loadComponent: () => {
          return import('./components/admin-view/employee-accounts/employee-accounts-main/employee-accounts-main.component')
          .then(m => m.EmployeeAccountsMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import('./components/admin-view/employee-accounts/manage-employees/manage-employees.component')
              .then(m => m.ManageEmployeesComponent);
            }
          },
          {
            path: 'manage-accounts',
            loadComponent: () => {
              return import('./components/admin-view/employee-accounts/manage-accounts/manage-accounts.component')
              .then(m => m.ManageAccountsComponent);
            }
          }
        ]
      }
    ]
  }
];
