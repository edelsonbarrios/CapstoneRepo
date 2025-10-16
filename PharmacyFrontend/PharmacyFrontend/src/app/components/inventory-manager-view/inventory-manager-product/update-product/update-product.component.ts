import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { Generic } from '../../../../models/generic-models/generic';
import { Classification } from '../../../../models/classification-models/classification';
import { TypeModel } from '../../../../models/type-models/type';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { Category } from '../../../../models/category-models/category';
import { CommonModule } from '@angular/common';
import { Brand } from '../../../../models/brand-models/brand';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { UpdateProductRequest } from '../../../../models/product-models/update-product-request';

@Component({
  selector: 'app-update-product',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './update-product.component.html',
  styleUrl: './update-product.component.scss'
})
export class UpdateProductComponent implements OnInit{
  @Input() productToUpdate: Product = new Product();
  @Output() closeForm = new EventEmitter<void>();
  generics: Generic[] = [];
  brands: Brand[] = [];
  classifications: Classification[] = [];
  types: TypeModel[] = [];
  formulations: Formulation[] = [];
  categories: Category[] = [];
  showSuccessAlert: boolean = false;

  constructor(
    private getService : GetService,
    private updateService: UpdateService
  ){}

  ngOnInit(): void {
    this.getAllItemDataControls();
  }

  updateProduct() {
    const requestPayload : UpdateProductRequest = {
      productName: this.productToUpdate.productName,
      categoryName: this.productToUpdate.categoryName,
      brandName: this.productToUpdate.brandName,
      genericName: this.productToUpdate.genericName,
      formulationName: this.productToUpdate.formulationName,
      classificationName: this.productToUpdate.classificationName,
      typeName: this.productToUpdate.typeName,
      sheetsPerBox: this.productToUpdate.sheetsPerBox,
      piecesPerSheet: this.productToUpdate.piecesPerSheet,
      boxPrice: this.productToUpdate.boxPrice,
      sheetPrice: this.productToUpdate.sheetPrice,
      piecePrice: this.productToUpdate.piecePrice,
      reorderLevel: this.productToUpdate.reorderLevel
    };
    this.updateService.updateProductAsync(this.productToUpdate.productID, requestPayload).subscribe({
      next: res => {
        this.showSuccessAlert = true;
      },
      error: err => {
        alert(`${err.error}`);
        console.log(this.productToUpdate);
      }
    });
  }

  closeUpdateProductForm() {
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeUpdateProductForm();
  }

  getAllItemDataControls() {
    this.getService.getAllGenericsAsync().subscribe({
      next: res => {
        this.generics = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    this.getService.getAllClassificationAsync().subscribe({
      next: res => {
        this.classifications = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    this.getService.getAllTypesAsync().subscribe({
      next: res => {
        this.types = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    this.getService.getAllFormulationsAsync().subscribe({
      next: res => {
        this.formulations = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    this.getService.getAllCategoriesAsync().subscribe({
      next: res => {
        this.categories = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
    this.getService.getAllBrandsAsync().subscribe({
      next: res => {
        this.brands = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}