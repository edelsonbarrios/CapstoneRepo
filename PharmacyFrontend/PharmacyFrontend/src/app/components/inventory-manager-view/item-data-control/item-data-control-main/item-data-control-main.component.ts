import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-item-data-control-main',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './item-data-control-main.component.html',
  styleUrl: './item-data-control-main.component.scss'
})
export class ItemDataControlMainComponent {
}