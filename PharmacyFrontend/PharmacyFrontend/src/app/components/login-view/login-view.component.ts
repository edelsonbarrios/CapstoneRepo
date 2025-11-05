import { Component, HostListener, OnInit } from '@angular/core';
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
export class LoginViewComponent implements OnInit {
  userName: string = '';
  password: string = '';
  loginError: string = '';
  isPreLoading: boolean = true;
  isLoggingIn: boolean = false;

  constructor(
    private postService: PostService,
    private router: Router
  ) {}

  ngOnInit() {
    // Pre-loading screen duration (2 seconds)
    setTimeout(() => {
      this.isPreLoading = false;
    }, 2000);
  }

  @HostListener('document:keydown.enter', ['$event'])
  onEnterPressed(event: KeyboardEvent) {
    if (!this.isPreLoading && !this.isLoggingIn) {
      this.Authenticate();
    }
  }

  Authenticate() {
    if (this.isLoggingIn) return;
    
    this.isLoggingIn = true;
    this.postService.Authentication(this.userName, this.password).subscribe({
      next: res => {
        this.loginError = '';
        localStorage.setItem(LogInToken, res.token);
        var token = localStorage.getItem(LogInToken);
        if(!token) {
          console.log('Token does not exist.');
          this.isLoggingIn = false;
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
        this.isLoggingIn = false;
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