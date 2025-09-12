import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Supplier } from '../../../../models/supplier-models/supplier';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-supplier',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-supplier.component.html',
  styleUrl: './create-supplier.component.scss'
})
export class CreateSupplierComponent {
  @Input() newSupplier = new Supplier();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService : PostService
  ) {}

  createNewSupplier() {
    this.postService.createSupplierAsync(this.newSupplier).subscribe({
      next: res => {
        alert('Supplier successfully created');
        console.log(this.newSupplier);
        this.closeForm.emit();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateSupplierForm() {
    this.closeForm.emit();
  }
}
