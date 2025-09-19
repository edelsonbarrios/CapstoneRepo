import { Component,EventEmitter,Input, Output } from '@angular/core';
import { Brand } from '../../../../models/brand-models/brand';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-brand.component.html',
  styleUrl: './add-brand.component.scss'
})
export class AddBrandComponent {
  @Input() addBrand: Brand = new Brand();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService: PostService
  ) {}

  createNewBrand()  {
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
    this.closeForm.emit();
  }
}
