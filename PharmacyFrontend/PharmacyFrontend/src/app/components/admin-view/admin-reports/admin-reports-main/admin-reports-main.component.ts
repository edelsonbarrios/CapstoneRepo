import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-reports-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './admin-reports-main.component.html',
  styleUrl: './admin-reports-main.component.scss'
})
export class AdminReportsMainComponent {
}