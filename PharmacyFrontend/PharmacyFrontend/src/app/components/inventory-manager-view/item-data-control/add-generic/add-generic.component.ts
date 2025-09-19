import { Component,EventEmitter,Input, Output} from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { Generic } from '../../../../models/generic-models/generic';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-generic',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-generic.component.html',
  styleUrl: './add-generic.component.scss'
})
export class AddGenericComponent {
  @Input() addGeneric: Generic = new Generic();
  @Output() closeForm = new EventEmitter<void>();

  constructor(
    private postService : PostService
  ) {}

  createNewGeneric() {
    this.postService.createGenericAsync(this.addGeneric).subscribe({
      next: res => {
        alert(`Generic sucesssfully created.`);
        this.closeAddGenericForm();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  closeAddGenericForm() {
    this.closeForm.emit();
  }
}
