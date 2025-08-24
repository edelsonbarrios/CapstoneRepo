import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Account } from '../../../../models/account';
import { PostService } from '../../../../services/PostService/post.service';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [NgIf],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})
export class CreateAccountComponent {
  @Input() newAccount = new Account();
  @Output() closeForm = new EventEmitter<void>();


  constructor(
    private postService : PostService
  ) {}

  closeCreateAccountForm() {
    this.closeForm.emit();
  }


}
