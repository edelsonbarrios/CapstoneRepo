import { Component,EventEmitter,Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Brand } from '../../../../models/brand-models/brand';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../services/UpdateService/update.service';

@Component({
  selector: 'app-edit-brand',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-brand.component.html',
  styleUrl: './edit-brand.component.scss'
})
export class EditBrandComponent implements OnInit{
  @Input() editBrand: Brand = new Brand();
  @Output() closeForm = new EventEmitter<void>();
  constructor(
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    console.log(this.editBrand);
  }

  updateBrand() {
    if(!this.editBrand) return;
    this.updateService.updateBrandAsync(this.editBrand.brandID, this.editBrand).subscribe({
      next: res => {
        alert(`Brand sucessfully updated.`);
        this.closeEditBrandForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.editBrand.brandName = '';
      }
    });
  }

  closeEditBrandForm() {
    this.closeForm.emit();
  }
}
