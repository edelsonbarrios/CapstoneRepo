import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgForOf } from '@angular/common';
import { Account } from '../../../../models/account';

@Component({
  selector: 'app-manage-accounts',
  standalone: true,
  imports: [NgForOf],
  templateUrl: './manage-accounts.component.html',
  styleUrl: './manage-accounts.component.scss'
})
export class ManageAccountsComponent implements OnInit{
  accounts: Account[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllAccounts();
  }

  getAllAccounts() {
    this.getService.GetAllAccounts().subscribe({
      next: accounts => {
        this.accounts = accounts;
        console.log(accounts);
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }


}
