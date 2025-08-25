import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-maintenance-main',
  standalone: true,
  imports: [RouterOutlet,RouterLink],
  templateUrl: './admin-maintenance-main.component.html',
  styleUrl: './admin-maintenance-main.component.scss'
})
export class AdminMaintenanceMainComponent {

}
