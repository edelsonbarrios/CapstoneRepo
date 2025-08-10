import { Component } from '@angular/core';
import { FormsModule} from '@angular/forms'
import { PostService } from '../../services/PostService/post.service';

@Component({
  selector: 'app-login-view',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login-view.component.html',
  styleUrl: './login-view.component.scss'
})
export class LoginViewComponent {
  userName: string = '';
  password: string = '';

  constructor(
    private postService: PostService
  ) {}

  Authenticate() {
    this.postService.Authentication(this.userName, this.password).subscribe({
      next: res => {
        console.log(`${res.message}`);
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}
