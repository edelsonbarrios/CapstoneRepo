import { Component, HostListener } from '@angular/core';
import { FormsModule} from '@angular/forms'
import { PostService } from '../../services/PostService/post.service';
import { Router } from '@angular/router';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login-view',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './login-view.component.html',
  styleUrl: './login-view.component.scss'
})
export class LoginViewComponent {
  userName: string = '';
  password: string = '';
  loginError: string = '';

  constructor(
    private postService: PostService,
    private router: Router
  ) {}

  @HostListener('document:keydown.enter', ['$event'])
  onEnterPressed(event: KeyboardEvent) {
    this.Authenticate();
  }

  Authenticate() {
    this.postService.Authentication(this.userName, this.password).subscribe({
      next: res => {
        this.loginError = '';
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
        }else if(rolePosition == "Cashier") {
          this.router.navigateByUrl('/cashier-main');
        }
      },
      error: err => {
        console.log(`${err.error.message}`);
        this.userName = '';
        this.password = '';
        this.loginError = "Invalid username or password";
      }
    });
  }

  goToAdminMain() {
    this.router.navigateByUrl('/admin-main');
  }

  goToInventoryMain() {
    this.router.navigateByUrl('/inventory-manager-main');
  }

  goToCashierMain() {
    this.router.navigateByUrl('/cashier-main');
  }
}
