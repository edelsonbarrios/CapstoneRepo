import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-employee',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.scss'
})
export class CreateEmployeeComponent {
  @Input() newEmployee = new Employee();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService : PostService
  ){}

  createNewEmployee() {
    this.newEmployee.birthDate = new Date(this.newEmployee.birthDate);
    this.postService.createEmployee(this.newEmployee).subscribe({
      next: res => {
        console.log(`${res.message}`);
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }

  clearFields() {
    this.newEmployee.employeeCode = ''
    this.newEmployee.lastName = '';
    this.newEmployee.firstName = '';
    this.newEmployee.middleName = '';
    this.newEmployee.age = 0;
    this.newEmployee.gender = '';
    this.newEmployee.phoneNumber = '';
    this.newEmployee.civilStatus = '';
    

  }

  closeCreateEmployeeForm() {
    this.closeForm.emit();
  }
}
