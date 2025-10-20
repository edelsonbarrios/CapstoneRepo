import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { VAT } from '../../../../models/vat-models/vat';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgForOf, NgIf } from "../../../../../../node_modules/@angular/common";
import { CreateVatRequest } from '../../../../models/vat-models/createVatRequest';
import { PostService } from '../../../../services/PostService/post.service';
import { Discount } from '../../../../models/discount-models/discount';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { Product } from '../../../../models/product-models/product';
import { EditVatComponent } from '../edit-vat/edit-vat.component';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';

@Component({
  selector: 'app-vat-entry',
  standalone: true,
  imports: [
    FormsModule, 
    NgForOf, 
    DatePipe, 
    NgIf, 
    EditVatComponent,
    ArchiveConfirmationComponent
  ],
  templateUrl: './vat-entry.component.html',
  styleUrl: './vat-entry.component.scss'
})
export class VatEntryComponent implements OnInit{
  vats : VAT[] = [];
  paginatedVats: VAT[] = [];
  createdVat : VAT = new VAT();
  vatError: string = '';
  editVat ?: VAT | null;
  showSuccessAlert: boolean = false;
  successMessage: string = '';
  showArchiveForm: number = 0;
  errorMessage: string = '';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 0;

  constructor(
    private getService: GetService,
    private postService: PostService,
    private updateService: UpdateService
  ){}

  ngOnInit(): void {
    this.getAllVat();
  }

  getAllVat(){
    this.getService.getAllVatAsync().subscribe({
      next: vats => {
        this.vats = vats;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
        this.errorMessage = err.error;
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.vats.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedVats = this.vats.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  createNewVat() {
    if(!this.createdVat.vatName) {
      this.vatError = 'VAT Name is required';
      return;
    }
    if (!this.createdVat.vatRate || this.createdVat.vatRate <= 0) {
      this.vatError = "Rate must be greater than 0";
      return;
    }
    this.vatError = '';
    const requestPayload: CreateVatRequest = {
      vatName : this.createdVat.vatName,
      vatRate : this.createdVat.vatRate,
    };
    this.postService.createVatAsync(requestPayload).subscribe({
      next: res => {
        this.successMessage = 'VAT has been Added Successfully';
        this.showSuccessAlert = true;
      },
      error: err => {
        this.createdVat.vatName = ''
        this.createdVat.vatRate = 0;
        this.errorMessage = err.error;
      }
    });
  }

  closeErrrMessage() {
    this.errorMessage = '';
  }

  openArchiveForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveForm() {
    this.showArchiveForm = 0;
  }

  archiveVat() {
    this.updateService.archiveVatAsync(this.showArchiveForm).subscribe({
      next: mes => {
        this.successMessage = 'VAT has been Archived Successfully';
        this.showArchiveForm = 0;
        this.getAllVat();
      },
      error: err => {
        alert(`${err.error.message}`);
      }
    });
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.createdVat.vatName = '';
    this.createdVat.vatRate = 0;
    this.currentPage = 1;
    this.getAllVat();
  }

  openEditVatForm(vat: VAT) {
    this.editVat = vat;
  }

  closeEditVatForm() {
    this.editVat = null;
  }
}