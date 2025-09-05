import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee';
import { FormsModule } from '@angular/forms';
import { dateTimestampProvider } from 'rxjs/internal/scheduler/dateTimestampProvider';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.scss'
})
export class ViewEmployeeComponent {
  @Input() employee : Employee = new Employee();
  @Output() closeForm = new EventEmitter<void>();
  isEditMode : boolean = false;

  constructor(

  ) {}

  closeViewForm() {
    this.closeForm.emit();
  }

  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
  }
}
