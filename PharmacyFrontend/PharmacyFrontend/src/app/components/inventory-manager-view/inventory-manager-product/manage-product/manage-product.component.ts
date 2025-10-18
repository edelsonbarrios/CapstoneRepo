import { Component, OnInit } from '@angular/core';
import { CreateProductComponent } from '../create-product/create-product.component';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { ViewProductComponent } from '../view-product/view-product.component';
import { GetService } from '../../../../services/GetService/get.service';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { UpdateProductComponent } from '../update-product/update-product.component';
import { ArchiveConfirmationComponent } from '../../../shared/archive-confirmation/archive-confirmation.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-product',
  standalone: true,
  imports: [
    CreateProductComponent, 
    NgIf, 
    ViewProductComponent, 
    NgFor, 
    UpdateProductComponent,
    ArchiveConfirmationComponent,
    FormsModule
  ],
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
  showArchiveForm : number = 0;
  message: string = 'Are you sure you want to delete this product?';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;
  searchQuery: string = '';

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

  get filteredProducts() : Product[] {
    if(!this.searchQuery.trim()) return this.products;
    const query = this.searchQuery.toLowerCase();
    return this.products.filter(p => p.productName.toLowerCase().includes(query));
  }

  onSearchChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    var items = this.filteredProducts;
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  openArchiveProductForm(id: number) {
    this.showArchiveForm = id;
  }

  closeArchiveProductForm() {
    this.showArchiveForm = 0;
  }

  archiveProduct(id : number) {
    if(id == null) return;
    this.updateService.archiveProductByIdAsync(id).subscribe({
      next: res => {
        this.showArchiveForm = 0;
        this.getAllProducts();
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