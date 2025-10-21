import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { Account } from '../../../../models/account';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-manage-accounts',
  standalone: true,
  imports: [NgForOf, FormsModule, NgIf],
  templateUrl: './manage-accounts.component.html',
  styleUrl: './manage-accounts.component.scss'
})
export class ManageAccountsComponent implements OnInit{
  accounts: Account[] = [];
  paginatedAccounts: Account[] = [];
  searchUsername: string = '';
  showSuccessMessage: string = '';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllAccounts();
  }

  get filteredAccounts() : Account[]{
    if(!this.searchUsername)
      return this.accounts;
    const query = this.searchUsername.toLowerCase();
    return this.accounts.filter(
      a => a.username.toLowerCase().includes(query)
    );
  }

  onSearchChange() {
    this.updatePagination();
  }

  getAllAccounts() {
    this.getService.GetAllAccounts().subscribe({
      next: accounts => {
        this.accounts = accounts;
        this.updatePagination();
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
        this.updatePagination();
        console.log(this.accounts);
        this.searchUsername = '';
      },
      error: err => {
        this.getAllAccounts();
        this.searchUsername = '';
      }
    });
  }

  updatePagination() {
    var items = this.filteredAccounts;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedAccounts = items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  resetPassword(id: number) {
    this.updateService.resetAccountPasswordAsync(id).subscribe({
      next: res => {
        this.showSuccessMessage = res.message;
      }
    })
  }

  closeResetPasswordMessage() {
    this.showSuccessMessage = '';
  }
}