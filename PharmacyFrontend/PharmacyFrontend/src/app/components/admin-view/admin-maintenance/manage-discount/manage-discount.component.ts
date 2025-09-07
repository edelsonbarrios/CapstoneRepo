import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { Discount } from '../../../../models/discount-models/discount';
import { CreateDiscountRequest } from '../../../../models/discount-models/create-discount';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-manage-discount',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './manage-discount.component.html',
  styleUrl: './manage-discount.component.scss',
})
export class ManageDiscountComponent implements OnInit {
  discounts: Discount[] = [];
  newDiscount: Discount = new Discount();
  errorMessage: string = '';

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getAllDiscount();
  }

  getAllDiscount() {
    this.getService.getAllDiscountAsync().subscribe({
      next: (discounts) => {
        this.discounts = discounts;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  createNewDiscount() {
    if (!this.newDiscount) return;

    if (!this.newDiscount.discountName || !this.newDiscount.discountRate) {
      this.errorMessage = 'Discount name and rate are required.';
      return;
    }

    const requestPayload: CreateDiscountRequest = {
      discountName: this.newDiscount.discountName,
      discountRate: this.newDiscount.discountRate,
    };

    this.postService.createDiscountAsync(requestPayload).subscribe({
      next: (res) => {
        this.errorMessage = '';
        alert('Discount successfully created.');
        this.newDiscount.discountName = '';
        this.newDiscount.discountRate = 0;
        this.getAllDiscount();
      },
      error: (err) => {
        alert(`${err.error}`);
        this.errorMessage = err.error;
      },
    });
  }
}
