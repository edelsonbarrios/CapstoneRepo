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
  
  // Error properties
  usernameError: string = '';
  passwordError: string = '';

  constructor(
    private postService : PostService
  ) {}

  closeCreateAccountForm() {
    // Clear errors when closing form
    this.usernameError = '';
    this.passwordError = '';
    this.closeForm.emit();
  }

  createNewAccount() {
    // Clear previous errors
    this.usernameError = '';
    this.passwordError = '';

    let hasErrors = false;

    // Validate username
    if (!this.newAccount.username || this.newAccount.username.trim() === '') {
      this.usernameError = 'Username is required';
      hasErrors = true;
    }

    // Validate password
    if (!this.newAccount.password || this.newAccount.password.trim() === '') {
      this.passwordError = 'Password is required';
      hasErrors = true;
    }

    if (hasErrors) {
      return;
    }

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