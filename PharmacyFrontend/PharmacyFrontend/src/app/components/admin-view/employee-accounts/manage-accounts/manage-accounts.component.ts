import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgForOf } from '@angular/common';
import { Account } from '../../../../models/account';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-accounts',
  standalone: true,
  imports: [NgForOf, FormsModule],
  templateUrl: './manage-accounts.component.html',
  styleUrl: './manage-accounts.component.scss'
})
export class ManageAccountsComponent implements OnInit{
  accounts: Account[] = [];
  searchUsername: string = '';

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

  searchAccountByUsername() {
    this.getService.SearchAccountByUserName(this.searchUsername).subscribe({
      next: acc => {
        this.accounts = acc ? [acc] : [];
        console.log(this.accounts);
        this.searchUsername = '';
      },
      error: err => {
        this.getAllAccounts();
        this.searchUsername = '';
      }
    });
  }

}
