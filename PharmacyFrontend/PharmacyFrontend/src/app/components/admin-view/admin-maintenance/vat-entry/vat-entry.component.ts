import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { VAT } from '../../../../models/vat-models/vat';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgForOf } from "../../../../../../node_modules/@angular/common";
import { CreateVatRequest } from '../../../../models/vat-models/createVatRequest';
import { PostService } from '../../../../services/PostService/post.service';

@Component({
  selector: 'app-vat-entry',
  standalone: true,
  imports: [FormsModule, NgForOf, DatePipe],
  templateUrl: './vat-entry.component.html',
  styleUrl: './vat-entry.component.scss'
})
export class VatEntryComponent implements OnInit{
  vats : VAT[] = [];
  createdVat : VAT = new VAT();

  constructor(
    private getService: GetService,
    private postService: PostService
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
}
