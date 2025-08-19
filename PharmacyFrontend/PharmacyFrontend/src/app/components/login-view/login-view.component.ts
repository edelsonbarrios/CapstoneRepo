import { Component } from '@angular/core';
import { FormsModule} from '@angular/forms'
import { PostService } from '../../services/PostService/post.service';
import { Router } from '@angular/router';
import { LogInToken } from '../../shared/token';

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
        
        if(res.token) {
          localStorage.setItem(LogInToken, res.token);
          this.router.navigateByUrl('/admin-main');
        }else {
          console.log(`${res.message}`);
        }
        
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
