import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { StockMovement } from '../../../../models/inventory-module-models/stock-movement';
import { CommonModule, DatePipe } from '@angular/common';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-view-adjustment',
  standalone: true,
  imports: [CommonModule, DatePipe, FormsModule],
  templateUrl: './view-adjustment.component.html',
  styleUrl: './view-adjustment.component.scss'
})
export class ViewAdjustmentComponent {
 @Input() viewAdjust: StockMovement = new StockMovement();
 @Output() closeForm = new EventEmitter<void>();

 constructor(
  private getService: GetService
 ) {}

 closeViewAdjustmentForm()  {
  this.closeForm.emit();
 }
}
