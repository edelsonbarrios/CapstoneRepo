import { Component, OnInit } from '@angular/core';
import { CreateProductComponent } from '../create-product/create-product.component';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { ViewProductComponent } from '../view-product/view-product.component';
import { GetService } from '../../../../services/GetService/get.service';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { UpdateProductComponent } from '../update-product/update-product.component';

@Component({
  selector: 'app-manage-product',
  standalone: true,
  imports: [CreateProductComponent, NgIf, ViewProductComponent, NgFor, UpdateProductComponent],
  templateUrl: './manage-product.component.html',
  styleUrl: './manage-product.component.scss'
})
export class ManageProductComponent implements OnInit{
  newProduct ?: Product | null;
  viewProduct ?: Product | null;
  updateProduct ?: Product | null;
  products : Product[] = [];
  paginatedProducts: Product[] = [];
  showSuccessAlert: boolean = false;
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;

  constructor(
    private getService : GetService,
    private updateService : UpdateService
  ) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  getAllProducts() {
    this.getService.getAllProductsAsync().subscribe({
      next: res => {
        this.products = res;
        this.updatePagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.products.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  archiveProduct(id : number) {
    if(id == null) return;
    this.updateService.archiveProductByIdAsync(id).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error.message}`);
      }
    });
  }

  openCreateProductForm(){
    this.newProduct= new Product();
  }

  openViewProductForm(product : Product) {
    this.viewProduct = product;
  }

  openUpdateProductForm(productToUpdate: Product) {
    this.updateProduct = productToUpdate;
  }

  closeUpdateProductForm() {
    this.updateProduct = null;
    this.getAllProducts();
  }

  closeViewProductForm() {
    this.viewProduct = null;
  }

  closeCreateProductForm() {
    this.newProduct = null;
    this.getAllProducts();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.currentPage = 1;
    this.getAllProducts();
  }
}