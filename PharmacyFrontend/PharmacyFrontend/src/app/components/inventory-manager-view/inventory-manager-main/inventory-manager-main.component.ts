import { Component } from '@angular/core';
import { InventoryManagerHeaderComponent } from "../inventory-manager-header/inventory-manager-header.component";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-inventory-manager-main',
  standalone: true,
  imports: [InventoryManagerHeaderComponent, RouterOutlet],
  templateUrl: './inventory-manager-main.component.html',
  styleUrl: './inventory-manager-main.component.scss'
})
export class InventoryManagerMainComponent {

}
