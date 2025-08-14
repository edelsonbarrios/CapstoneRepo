import { Component } from '@angular/core';
import { FormsModule} from '@angular/forms'
import { PostService } from '../../services/PostService/post.service';
import { Router } from '@angular/router';

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
    private postService: PostService,
    private router: Router
  ) {}

  Authenticate() {
    this.postService.Authentication(this.userName, this.password).subscribe({
      next: res => {
        console.log(`${res.message}`);
        this.router.navigateByUrl('/admin-main');
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  goToAdminMain() {
    this.router.navigateByUrl('/admin-main');
  }
}
