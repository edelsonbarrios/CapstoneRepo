import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';

@Component({
  selector: 'app-create-supplier',
  standalone: true,
  imports: [],
  templateUrl: './create-supplier.component.html',
  styleUrl: './create-supplier.component.scss'
})
export class CreateSupplierComponent {
  @Input() newSupplier = new Supplier();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    
  ) {}
}
