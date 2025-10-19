import { Component,EventEmitter,Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Brand } from '../../../../models/brand-models/brand';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-brand',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './edit-brand.component.html',
  styleUrl: './edit-brand.component.scss'
})
export class EditBrandComponent implements OnInit{
  @Input() editBrand: Brand = new Brand();
  @Output() closeForm = new EventEmitter<void>();
  
  brandNameError: string = '';
  showSuccessAlert: boolean = false;

  constructor(
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    console.log(this.editBrand);
  }

  updateBrand() {
    // Clear previous error
    this.brandNameError = '';
    
    // Validate brand name
    if (!this.editBrand.brandName || this.editBrand.brandName.trim() === '') {
      this.brandNameError = 'Brand name is required';
      return;
    }

    if(!this.editBrand) return;
    this.updateService.updateBrandAsync(this.editBrand.brandID, this.editBrand).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);
        this.editBrand.brandName = '';
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeEditBrandForm();
  }

  closeEditBrandForm() {
    // Clear error when closing form
    this.brandNameError = '';
    this.closeForm.emit();
  }
}