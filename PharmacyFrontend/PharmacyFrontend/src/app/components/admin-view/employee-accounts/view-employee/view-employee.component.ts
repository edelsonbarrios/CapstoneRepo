import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.scss'
})
export class ViewEmployeeComponent {
  @Input() employee : Employee = new Employee();
  @Output() closeForm = new EventEmitter<void>();

  constructor(

  ) {}

  showViewEmployeeForm() {

  }
}
