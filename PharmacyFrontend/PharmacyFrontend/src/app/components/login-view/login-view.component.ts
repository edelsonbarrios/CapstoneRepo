import { Component } from '@angular/core';
import { FormsModule} from '@angular/forms'
import { PostService } from '../../services/PostService/post.service';
import { Router } from '@angular/router';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';

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
        console.log(`${res.token}`);
        localStorage.setItem(LogInToken, res.token);
        var token = localStorage.getItem(LogInToken);
        if(!token) {
          console.log('Token does not exist.');
          return
        }
        const employee: any = jwtDecode(token);
        const rolePosition = employee["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        if(rolePosition == "Administrator") {
          this.router.navigateByUrl('/admin-main');
        }else if(rolePosition == "Inventory Manager") {
          this.router.navigateByUrl('/inventory-manager-main');
        }   
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }

  goToAdminMain() {
    this.router.navigateByUrl('/admin-main');
  }
}
