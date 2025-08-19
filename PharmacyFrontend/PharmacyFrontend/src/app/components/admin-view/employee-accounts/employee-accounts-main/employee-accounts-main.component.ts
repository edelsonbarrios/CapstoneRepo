import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-employee-accounts-main',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './employee-accounts-main.component.html',
  styleUrl: './employee-accounts-main.component.scss'
})
export class EmployeeAccountsMainComponent{

  constructor(
  ) {}
}
