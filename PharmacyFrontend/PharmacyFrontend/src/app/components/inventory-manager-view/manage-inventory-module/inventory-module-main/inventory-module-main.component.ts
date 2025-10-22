import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-inventory-module-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './inventory-module-main.component.html',
  styleUrl: './inventory-module-main.component.scss'
})
export class InventoryModuleMainComponent {
}