import { Component, EventEmitter, Input, Output, Type } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { TypeModel } from '../../../../models/type-models/type';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-type',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-type.component.html',
  styleUrl: './add-type.component.scss'
})
export class AddTypeComponent {
  @Input() addType: TypeModel = new TypeModel();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService : PostService
  ) {}

  createNewType() {
    this.postService.createNewTypeAsync(this.addType).subscribe({
      next: res => {
        alert(`Type successsfully created.`);
        this.closeAddTypeForm();
      },
      error: err => {
        console.log(`${err.error}`);
        this.addType.typeName = '';
      }
    });
  }

  closeAddTypeForm() {
    this.closeForm.emit();
  }
}
