import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => {
      return import('./components/login-view/login-view.component').then(
        (m) => m.LoginViewComponent
      );
    },
  },
  {
    path: 'log-in',
    loadComponent: () => {
      return import('./components/login-view/login-view.component').then(
        (m) => m.LoginViewComponent
      );
    },
  },
  {
    path: 'admin-main',
    loadComponent: () => {
      return import(
        './components//admin-view/admin-main/admin-main.component'
      ).then((m) => m.AdminMainComponent);
    },
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => {
          return import(
            './components/admin-view/admin-dashboard/admin-dashboard.component'
          ).then((m) => m.AdminDashboardComponent);
        },
      },
      {
        path: 'admin-dashboard',
        loadComponent: () => {
          return import(
            './components/admin-view/admin-dashboard/admin-dashboard.component'
          ).then((m) => m.AdminDashboardComponent);
        },
      },
      {
        path: 'admin-reports-main',
        loadComponent: () => {
          return import(
            './components/admin-view/admin-reports/admin-reports-main/admin-reports-main.component'
          ).then((m) => m.AdminReportsMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-reports/sales-reports/sales-reports.component'
              ).then((m) => m.SalesReportsComponent);
            },
          },
          {
            path: 'sales-reports',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-reports/sales-reports/sales-reports.component'
              ).then((m) => m.SalesReportsComponent);
            },
          },
          {
            path: 'inventory-reports',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-reports/inventory-reports/inventory-reports.component'
              ).then((m) => m.InventoryReportsComponent);
            },
          },
          {
            path: 'fast-moving-products',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-reports/fast-moving-products/fast-moving-products.component'
              ).then((m) => m.FastMovingProductsComponent);
            },
          },
        ],
      },
      {
        path: 'admin-maintenance-main',
        loadComponent: () => {
          return import(
            './components/admin-view/admin-maintenance/admin-maintenance-main/admin-maintenance-main.component'
          ).then((m) => m.AdminMaintenanceMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-maintenance/vat-entry/vat-entry.component'
              ).then((m) => m.VatEntryComponent);
            },
          },
          {
            path: 'vat-entry',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-maintenance/vat-entry/vat-entry.component'
              ).then((m) => m.VatEntryComponent);
            },
          },
          {
            path: 'database-backup',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-maintenance/database-backup/database-backup.component'
              ).then((m) => m.DatabaseBackupComponent);
            },
          },
          {
            path: 'manage-discount',
            loadComponent: () => {
              return import(
                './components/admin-view/admin-maintenance/manage-discount/manage-discount.component'
              ).then((m) => m.ManageDiscountComponent);
            },
          },
        ],
      },
      {
        path: 'employee-accounts-main',
        loadComponent: () => {
          return import(
            './components/admin-view/employee-accounts/employee-accounts-main/employee-accounts-main.component'
          ).then((m) => m.EmployeeAccountsMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/admin-view/employee-accounts/manage-employees/manage-employees.component'
              ).then((m) => m.ManageEmployeesComponent);
            },
          },
          {
            path: 'manage-employees',
            loadComponent: () => {
              return import(
                './components/admin-view/employee-accounts/manage-employees/manage-employees.component'
              ).then((m) => m.ManageEmployeesComponent);
            },
          },
          {
            path: 'manage-accounts',
            loadComponent: () => {
              return import(
                './components/admin-view/employee-accounts/manage-accounts/manage-accounts.component'
              ).then((m) => m.ManageAccountsComponent);
            },
          },
          {
            path: 'manage-employee-history',
            loadComponent: () => {
              return import(
                './components/admin-view/employee-accounts/manage-employee-history/manage-employee-history.component'
              ).then((m) => m.ManageEmployeeHistoryComponent);
            },
          },
        ],
      },
      {
        path: 'admin-supplier',
        loadComponent: () => {
          return import(
            './components/admin-view/supplier/admin-supplier/admin-supplier.component'
          ).then((m) => m.AdminSupplierComponent);
        },
      },
      {
        path: 'create-supplier',
        loadComponent: () => {
          return import(
            './components/admin-view/supplier/create-supplier/create-supplier.component'
          ).then((m) => m.CreateSupplierComponent);
        },
      },
    ],
  },
  {
    path: 'inventory-manager-main',
    loadComponent: () => {
      return import(
        './components/inventory-manager-view/inventory-manager-main/inventory-manager-main.component'
      ).then((m) => m.InventoryManagerMainComponent);
    },
    children: [
      {
        path: '',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-dashboard/inventory-manager-dashboard.component'
          ).then((m) => m.InventoryManagerDashboardComponent);
        },
      },
      {
        path: 'inventory-manager-header',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-header/inventory-manager-header.component'
          ).then((m) => m.InventoryManagerHeaderComponent);
        },
      },
      {
        path: 'inventory-manager-dashboard',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-dashboard/inventory-manager-dashboard.component'
          ).then((m) => m.InventoryManagerDashboardComponent);
        },
      },
      {
        path: 'inventory-manager-product-main',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-product/inventory-manager-product-main/inventory-manager-product-main.component'
          ).then((m) => m.InventoryManagerProductMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/inventory-manager-product/manage-product/manage-product.component'
              ).then((m) => m.ManageProductComponent);
            },
          },
          {
            path: 'manage-product',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/inventory-manager-product/manage-product/manage-product.component'
              ).then((m) => m.ManageProductComponent);
            },
          },
          {
            path: 'manage-category',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/inventory-manager-product/manage-category/manage-category.component'
              ).then((m) => m.ManageCategoryComponent);
            },
          },
        ],
      },
      {
        path: 'inventory-purchase-order',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-purchase-order/inventory-purchase-order/inventory-purchase-order.component'
          ).then((m) => m.InventoryPurchaseOrderComponent);
        },
      },
      {
        path: 'purchase-order-products',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-purchase-order/purchase-order-products/purchase-order-products.component'
          ).then((m) => m.PurchaseOrderProductsComponent);
        },
      },
      {
        path: 'manage-replenishment',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/inventory-manager-replenishment/manage-replenishment/manage-replenishment.component'
          ).then((m) => m.ManageReplenishmentComponent);
        },
      },
      {
        path: 'inventory-module-main',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/manage-inventory-module/inventory-module-main/inventory-module-main.component'
          ).then((m) => m.InventoryModuleMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/manage-inventory-module/manage-stock-entry/manage-stock-entry.component'
              ).then((m) => m.ManageStockEntryComponent);
            },
          },
          {
            path: 'manage-stock-entry',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/manage-inventory-module/manage-stock-entry/manage-stock-entry.component'
              ).then((m) => m.ManageStockEntryComponent);
            },
          },
          {
            path: 'manage-stock-adjustment',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/manage-inventory-module/manage-stock-adjustment/manage-stock-adjustment.component'
              ).then((m) => m.ManageStockAdjustmentComponent);
            },
          },
        ],
      },
      {
        path: 'item-data-control-main',
        loadComponent: () => {
          return import(
            './components/inventory-manager-view/item-data-control/item-data-control-main/item-data-control-main.component'
          ).then((m) => m.ItemDataControlMainComponent);
        },
        children: [
          {
            path: '',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-brand-name/manage-brand-name.component'
              ).then((m) => m.ManageBrandNameComponent);
            },
          },
          {
            path: 'manage-brand-name',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-brand-name/manage-brand-name.component'
              ).then((m) => m.ManageBrandNameComponent);
            },
          },
          {
            path: 'manage-generic-name',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-generic-name/manage-generic-name.component'
              ).then((m) => m.ManageGenericNameComponent);
            },
          },
          {
            path: 'manage-formulation',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-formulation/manage-formulation.component'
              ).then((m) => m.ManageFormulationComponent);
            },
          },
          {
            path: 'manage-classification',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-classification/manage-classification.component'
              ).then((m) => m.ManageClassificationComponent);
            },
          },
          {
            path: 'manage-type',
            loadComponent: () => {
              return import(
                './components/inventory-manager-view/item-data-control/manage-type/manage-type.component'
              ).then((m) => m.ManageTypeComponent);
            },
          },
        ],
      },
    ],
  },
  {
    path: 'cashier-main',
    loadComponent: () => {
      return import(
        './components/cashier-view/cashier-main/cashier-main.component'
      ).then((m) => m.CashierMainComponent);
    },
    children: [
      {
        path: '',
        loadComponent: () => {
          return import(
            './components/cashier-view/cashier-dashboard/cashier-dashboard.component'
          ).then((m) => m.CashierDashboardComponent);
        }
      },
      {
        path: 'cashier-dashboard',
        loadComponent: () => {
          return import(
            './components/cashier-view/cashier-dashboard/cashier-dashboard.component'
          ).then((m) => m.CashierDashboardComponent);
        }
      },
      {
        path: 'cashier-header',
        loadComponent: () => {
          return import(
            './components/cashier-view/cashier-header/cashier-header.component'
          ).then((m) => m.CashierHeaderComponent);
        }
      },
      {
        path: 'cashier-transaction',
        loadComponent: () => {
          return import(
            './components/cashier-view/cashier-transaction/cashier-transaction.component'
          ).then((m) => m.CashierTransactionComponent);
        }
      }
    ],
  },
];
