import { Component, EventEmitter, Input, Output, Type } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { TypeModel } from '../../../../models/type-models/type';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-type',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-type.component.html',
  styleUrl: './add-type.component.scss'
})
export class AddTypeComponent {
  @Input() addType: TypeModel = new TypeModel();
  @Output() closeForm = new EventEmitter<void>();
  
  typeNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private postService : PostService
  ) {}

  createNewType() {
    // Clear previous error
    this.typeNameError = '';
    
    // Validate type name
    if (!this.addType.typeName || this.addType.typeName.trim() === '') {
      this.typeNameError = 'Type name is required';
      return;
    }

    this.postService.createNewTypeAsync(this.addType).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        this.addType.typeName = '';
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeAddTypeForm();
  }

  closeAddTypeForm() {
    // Clear error when closing form
    this.typeNameError = '';
    this.closeForm.emit();
  }
}