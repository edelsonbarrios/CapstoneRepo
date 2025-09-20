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

@Component({
  selector: 'app-vat-entry',
  standalone: true,
  imports: [FormsModule, NgForOf, DatePipe, NgIf, EditVatComponent],
  templateUrl: './vat-entry.component.html',
  styleUrl: './vat-entry.component.scss'
})
export class VatEntryComponent implements OnInit{
  vats : VAT[] = [];
  createdVat : VAT = new VAT();
  vatError: string = '';
  editVat ?: Product | null;

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
        this.vats = vats
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
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
        alert(`VAT successfully created.`);
        this.createdVat.vatName = ''
        this.createdVat.vatRate = 0;
        this.getAllVat();
      },
      error: err => {
        alert(`${err.error}`);
        this.createdVat.vatName = ''
        this.createdVat.vatRate = 0;
      }
    });
  }

  archiveVat(vat : VAT) {
    if(!vat) return;
    this.updateService.archiveVatAsync(vat.vatid).subscribe({
      next: mes => {
        alert(`${mes.message}`);
        this.getAllVat();
      },
      error: err => {
        alert(`${err.error.message}`);
      }
    });
  }

   openEditVatForm() {
    this.editVat = new Product();
  }
}
