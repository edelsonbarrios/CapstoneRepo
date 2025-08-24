import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../../../models/employee';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf} from "../../../../../../node_modules/@angular/common";
import { Account } from '../../../../models/account';
import { CreateAccountComponent } from '../create-account/create-account.component';

@Component({
  selector: 'app-create-employee',
  standalone: true,
  imports: [FormsModule, NgIf, CreateAccountComponent],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.scss'
})
export class CreateEmployeeComponent {
  @Input() newEmployee : Employee | null = new Employee();
  @Output() closeForm = new EventEmitter<number>();
  newAccount ?: Account | null;
  newEmployeeId : number = 0;

  constructor(
    private postService : PostService
  ){}

  createNewEmployee() {
    this.postService.createEmployee(this.newEmployee).subscribe({
      next: res => {
        alert(`${res.message}`);
        this.newEmployeeId = res.employeeId;
        this.clearFields();
        this.openCreateAccountForm();
        this.newEmployee = null;
      },
      error: err => {
        alert(`${err.error.message}`);
      }
    });
  }

  clearFields() {
    if (!this.newEmployee) {
      this.newEmployee = new Employee(); 
    }
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

  openCreateAccountForm() {
    this.newAccount = new Account();
  }

  closeCreateAccountForm() {
    this.newAccount = null;
  }
}
