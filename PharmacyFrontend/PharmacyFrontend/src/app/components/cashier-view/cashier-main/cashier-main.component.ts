import { Component } from '@angular/core';
import { CashierHeaderComponent } from '../cashier-header/cashier-header.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-cashier-main',
  standalone: true,
  imports: [CashierHeaderComponent, RouterOutlet],
  templateUrl: './cashier-main.component.html',
  styleUrl: './cashier-main.component.scss'
})
export class CashierMainComponent {

}
