import { Component,EventEmitter,Input, Output} from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Generic } from '../../../../models/generic-models/generic';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-generic',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-generic.component.html',
  styleUrl: './add-generic.component.scss'
})
export class AddGenericComponent {
  @Input() addGeneric: Generic = new Generic();
  @Output() closeForm = new EventEmitter<void>();
  
  genericNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private postService : PostService
  ) {}

  createNewGeneric() {
    // Clear previous error
    this.genericNameError = '';
    
    // Validate generic name
    if (!this.addGeneric.genericName || this.addGeneric.genericName.trim() === '') {
      this.genericNameError = 'Generic name is required';
      return;
    }

    this.postService.createGenericAsync(this.addGeneric).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeAddGenericForm();
  }

  closeAddGenericForm() {
    // Clear error when closing form
    this.genericNameError = '';
    this.closeForm.emit();
  }
}