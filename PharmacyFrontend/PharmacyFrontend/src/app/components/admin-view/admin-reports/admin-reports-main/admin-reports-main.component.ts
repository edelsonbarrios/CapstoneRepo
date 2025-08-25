import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-reports-main',
  standalone: true,
  imports: [RouterOutlet,RouterLink],
  templateUrl: './admin-reports-main.component.html',
  styleUrl: './admin-reports-main.component.scss'
})
export class AdminReportsMainComponent {

}
