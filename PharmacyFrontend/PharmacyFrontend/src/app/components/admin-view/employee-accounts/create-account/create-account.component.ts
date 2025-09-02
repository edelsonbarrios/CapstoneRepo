import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Account } from '../../../../models/account';
import { PostService } from '../../../../services/PostService/post.service';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [NgIf, FormsModule],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})
export class CreateAccountComponent {
  @Input() employeeId : number = 0;
  @Output() closeForm = new EventEmitter<void>();
  newAccount : Account = new Account();

  constructor(
    private postService : PostService
  ) {}

  closeCreateAccountForm() {
    this.closeForm.emit();
  }

  createNewAccount() {
    this.newAccount.employeeId = this.employeeId;
    this.postService.createAccount(this.newAccount).subscribe({
      next: res => {
        console.log(`${res.message}`);
        this.newAccount.username = '';
        this.newAccount.password = '';
        this.closeCreateAccountForm();
      },
      error: err => {
        console.log(err.error.message);
        this.newAccount.username = '';
        this.newAccount.password = '';
      }
    });
  }
}