import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UpdateService } from '../../../services/UpdateService/update.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-archive-confirmation',
  standalone: true,
  imports: [NgIf],
  templateUrl: './archive-confirmation.component.html',
  styleUrls: ['./archive-confirmation.component.scss']
})
export class ArchiveConfirmationComponent {
  @Input() id: number = 0;
  @Input() alertMessage : string = '';

  @Input() message: string = '';
  @Input() successMessage : string = '';

  @Output() onArchive = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  showSuccessAlert : boolean = false;


  constructor(
   private updateService: UpdateService
  ) {}

  onCommit() {
    this.showSuccessAlert = true;
  }

  closeArchiveForm() {
    this.onCancel.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.onArchive.emit();
  }
}
