import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-inventory-manager-product-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './inventory-manager-product-main.component.html',
  styleUrl: './inventory-manager-product-main.component.scss'
})
export class InventoryManagerProductMainComponent {

}
