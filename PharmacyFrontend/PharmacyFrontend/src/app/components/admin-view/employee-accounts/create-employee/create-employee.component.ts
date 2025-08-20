import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee';

@Component({
  selector: 'app-create-employee',
  standalone: true,
  imports: [],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.scss'
})
export class CreateEmployeeComponent {
  @Input() newEmployee = new Employee();
  @Output() closeForm = new EventEmitter<void>();

  constructor(

  ){}

  closeCreateEmployeeForm() {
    this.closeForm.emit();
  }
}
