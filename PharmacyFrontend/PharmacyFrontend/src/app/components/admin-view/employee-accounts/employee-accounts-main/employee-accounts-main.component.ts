import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-employee-accounts-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './employee-accounts-main.component.html',
  styleUrl: './employee-accounts-main.component.scss'
})
export class EmployeeAccountsMainComponent {
}