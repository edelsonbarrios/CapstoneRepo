import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Inventory } from '../../../../models/inventory-module-models/inventory';

@Component({
  selector: 'app-inventory-module-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './inventory-module-main.component.html',
  styleUrl: './inventory-module-main.component.scss'
})
export class InventoryModuleMainComponent {
  @Input() loginVerify: Inventory = new Inventory();
}