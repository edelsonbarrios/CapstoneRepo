import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-item-data-control-main',
  standalone: true,
  imports: [RouterOutlet,RouterLink],
  templateUrl: './item-data-control-main.component.html',
  styleUrl: './item-data-control-main.component.scss'
})
export class ItemDataControlMainComponent {

}
