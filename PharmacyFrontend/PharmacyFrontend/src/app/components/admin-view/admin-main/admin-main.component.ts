import { Component } from '@angular/core';
import { AdminHeaderComponent } from '../admin-header/admin-header.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-main',
  standalone: true,
  imports: [AdminHeaderComponent, RouterOutlet],
  templateUrl: './admin-main.component.html',
  styleUrls: ['./admin-main.component.scss']
})
export class AdminMainComponent {
  

  constructor(

  ) {}
}
