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
        path: '',
        loadComponent: () => {
          return import('./components/admin-view/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent);
        }
      }
    ]
  }
];
