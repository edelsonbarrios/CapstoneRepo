import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { CreateProductRequest } from '../../../../models/product-models/create-product-request';
import { Category } from '../../../../models/category-models/category';
import { GetService } from '../../../../services/GetService/get.service';
import { NgFor, NgIf } from '@angular/common';
import { Brand } from '../../../../models/brand-models/brand';
import { Generic } from '../../../../models/generic-models/generic';
import { TypeModel } from '../../../../models/type-models/type';
import { Formulation } from '../../../../models/formulation-models/formulation';
import { Classification } from '../../../../models/classification-models/classification';
import { Supplier } from '../../../../models/supplier-models/supplier';

@Component({
  selector: 'app-create-product',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.scss'
})
export class CreateProductComponent implements OnInit{
  @Input() newProduct: Product = new Product;
  @Output() closeForm = new EventEmitter<void>();
  categories : Category[] = [];
  brands: Brand[] = [];
  generics : Generic[] = [];
  types: TypeModel[] = [];
  formulations : Formulation[] = [];
  classifications : Classification[] = [];
  suppliers : Supplier[] = [];
  showSuccessAlert: boolean = false;
  unitMeasurement: string = 'Multi-Level';

  // Validation error properties
  productNameError: string = '';
  categoryNameError: string = '';
  unitMeasurementError: string = '';
  sheetsPerBoxError: string = '';
  piecesPerSheetError: string = '';
  piecePerBoxError: string = '';
  boxPriceError: string = '';
  piecePriceError: string = '';
  reorderLevelError: string = '';

  constructor(
    private postService : PostService,
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.newProduct.unitMeasurement = 'Multi-Level';
    this.newProduct.categoryName = 'Medicine';
    this.getAllCategories();
    this.getAllBrands();
    this.getAllGenerics();
    this.getAllTypeModels();
    this.getAllFormulation();
    this.getAllClassification();
    this.getAllSuppliers();
  }

  getAllCategories() {
    this.getService.getAllCategoriesAsync().subscribe({
      next: categories => {
        this.categories = categories;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllBrands() {
    this.getService.getAllBrandsAsync().subscribe({
      next: brands => {
        this.brands = brands;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllGenerics() {
    this.getService.getAllGenericsAsync().subscribe({
      next: generics => {
        this.generics = generics;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllTypeModels() {
    this.getService.getAllTypesAsync().subscribe({
      next: types => {
        this.types = types;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllFormulation() {
    this.getService.getAllFormulationsAsync().subscribe({
      next: res => {
        this.formulations = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllClassification() {
    this.getService.getAllClassificationAsync().subscribe({
      next: res => {
        this.classifications = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getAllSuppliers() {
    this.getService.getAllSuppliersAsync().subscribe({
      next: res => {
        this.suppliers = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  createProduct() {
    if(!this.newProduct) return;
    const requestPayload : CreateProductRequest = {
      productName: this.newProduct.productName,
      categoryName : this.newProduct.categoryName,
      brandName : this.newProduct.brandName,
      genericName : this.newProduct.genericName,
      formulationName : this.newProduct.formulationName,
      classificationName : this.newProduct.classificationName,
      typeName : this.newProduct.typeName,
      sheetsPerBox: this.newProduct.sheetsPerBox,
      piecesPerSheet: this.newProduct.piecesPerSheet,
      boxPrice: this.newProduct.boxPrice,
      sheetPrice: this.newProduct.sheetPrice,
      piecePrice: this.newProduct.piecePrice,
      reorderLevel: this.newProduct.reorderLevel,
      unitMeasurement: this.newProduct.unitMeasurement,
      piecePerBox: this.newProduct.piecePerBox,
    };
    this.postService.createProductAsync(requestPayload).subscribe({
      next: res => {
        debugger;
        console.log(`${JSON.stringify(res)}`);
        this.showSuccessAlert = true;
        console.log(`papat`);
      },
      error: err => {
        alert(`${err.error}`);
        console.log(`${err.error}`);
      }
    });
  }

  closeCreateProductForm() {
    // Clear all errors when closing form
    this.productNameError = '';
    this.categoryNameError = '';
    this.unitMeasurementError = '';
    this.sheetsPerBoxError = '';
    this.piecesPerSheetError = '';
    this.piecePerBoxError = '';
    this.boxPriceError = '';
    this.piecePriceError = '';
    this.reorderLevelError = '';
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateProductForm();
  }

  selectUnitOfMeasurement(measurement: string) {
    this.unitMeasurement = measurement;
  }
}