import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddBrandComponent } from '../add-brand/add-brand.component';
import { EditBrandComponent } from '../edit-brand/edit-brand.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Brand } from '../../../../models/brand-models/brand';

@Component({
  selector: 'app-manage-brand-name',
  standalone: true,
  imports: [NgIf, AddBrandComponent, EditBrandComponent, NgFor],
  templateUrl: './manage-brand-name.component.html',
  styleUrl: './manage-brand-name.component.scss',
})
export class ManageBrandNameComponent implements OnInit{
  addBrand?: Brand | null;
  editBrand?: Product | null;
  brands: Brand[] = [];

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getAllBrands();
  }

  getAllBrands() {
    this.getService.getAllBrandsAsync().subscribe({
      next: (res) => {
        this.brands = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  openAddBrandForm() {
    this.addBrand = new Brand();
  }

  closeAddBrandForm() {
    this.addBrand = null;
    this.getAllBrands();
  }

  openEditBrandForm() {
    this.editBrand = new Product();
  }
}
