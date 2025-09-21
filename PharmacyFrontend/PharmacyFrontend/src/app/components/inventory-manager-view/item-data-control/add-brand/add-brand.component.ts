import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Brand } from '../../../../models/brand-models/brand';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './add-brand.component.html',
  styleUrl: './add-brand.component.scss'
})
export class AddBrandComponent {
  @Input() addBrand: Brand = new Brand();
  @Output() closeForm = new EventEmitter<void>();
  
  brandNameError: string = '';

  constructor(
    private postService: PostService
  ) {}

  createNewBrand() {
    // Clear previous error
    this.brandNameError = '';
    
    // Validate brand name
    if (!this.addBrand.brandName || this.addBrand.brandName.trim() === '') {
      this.brandNameError = 'Brand name is required';
      return;
    }

    this.postService.createBrandAsync(this.addBrand).subscribe({
      next: res => {
        alert(`Brand successfully created.`);
        this.closeCreateBrandForm();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateBrandForm() {
    // Clear error when closing form
    this.brandNameError = '';
    this.closeForm.emit();
  }
}