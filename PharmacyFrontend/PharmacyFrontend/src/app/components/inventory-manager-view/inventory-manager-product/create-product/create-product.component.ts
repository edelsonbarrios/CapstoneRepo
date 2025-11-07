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
export class CreateProductComponent implements OnInit {
  @Input() newProduct: Product = new Product();
  @Output() closeForm = new EventEmitter<void>();

  categories: Category[] = [];
  brands: Brand[] = [];
  generics: Generic[] = [];
  types: TypeModel[] = [];
  formulations: Formulation[] = [];
  classifications: Classification[] = [];
  suppliers: Supplier[] = [];

  showSuccessAlert: boolean = false;
  unitMeasurement: string = 'Multi-Level';

  // Validation error properties
  productNameError: string = '';
  categoryNameError: string = '';
  unitMeasurementError: string = '';
  sheetsPerBoxError: string = '';
  piecesPerSheetError: string = '';
  piecePerBoxError: string = '';
  costBoxPriceError: string = '';
  costPiecePriceError: string = '';
  boxPriceError: string = '';
  piecePriceError: string = '';
  reorderLevelError: string = '';

  constructor(
    private postService: PostService,
    private getService: GetService
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

  calculateBoxSheetPrice() {
    if (this.unitMeasurement === 'Multi-Level') {
      // Auto-calculate sheet and box selling prices
      if (this.newProduct.piecesPerSheet && this.newProduct.piecePrice) {
        this.newProduct.sheetPrice = this.newProduct.piecePrice * this.newProduct.piecesPerSheet;
      }
      if (this.newProduct.sheetsPerBox && this.newProduct.sheetPrice) {
        this.newProduct.boxPrice = this.newProduct.sheetPrice * this.newProduct.sheetsPerBox;
      }
    } else if (this.unitMeasurement === 'Dual-Level') {
      // Auto-calculate box selling price
      if (this.newProduct.piecePerBox && this.newProduct.piecePrice) {
        this.newProduct.boxPrice = this.newProduct.piecePrice * this.newProduct.piecePerBox;
      }
    }
  }

  createProduct() {
    // Clear all previous errors
    this.clearAllErrors();

    console.log('Cost Box Price:', this.newProduct.costBoxPrice);
    console.log('Cost Sheet Price:', this.newProduct.costSheetPrice);
    console.log('Cost Piece Price:', this.newProduct.costPiecePrice);

    let hasError = false;

    // Product Name validation
    if (!this.newProduct.productName || this.newProduct.productName.trim() === '') {
      this.productNameError = 'Product name is required';
      hasError = true;
    }

    // Category Name validation
    if (!this.newProduct.categoryName || this.newProduct.categoryName === '') {
      this.categoryNameError = 'Category is required';
      hasError = true;
    }

    // Unit Measurement validation
    if (!this.newProduct.unitMeasurement) {
      this.unitMeasurementError = 'Unit packaging is required';
      hasError = true;
    }

    // Multi-Level specific validations
    if (this.unitMeasurement === 'Multi-Level') {
      if (!this.newProduct.sheetsPerBox || this.newProduct.sheetsPerBox <= 0) {
        this.sheetsPerBoxError = 'Sheets per box is required and must be greater than 0';
        hasError = true;
      }
      if (!this.newProduct.piecesPerSheet || this.newProduct.piecesPerSheet <= 0) {
        this.piecesPerSheetError = 'Pieces per sheet is required and must be greater than 0';
        hasError = true;
      }
    }

    // Dual-Level specific validations
    if (this.unitMeasurement === 'Dual-Level') {
      if (!this.newProduct.piecePerBox || this.newProduct.piecePerBox <= 0) {
        this.piecePerBoxError = 'Pieces per box is required and must be greater than 0';
        hasError = true;
      }
    }

    // Cost Box Price validation (Multi-Level & Dual-Level)
    if (this.unitMeasurement === 'Multi-Level' || this.unitMeasurement === 'Dual-Level') {
      if (!this.newProduct.costBoxPrice || this.newProduct.costBoxPrice <= 0) {
        this.costBoxPriceError = 'Cost box price is required';
        hasError = true;
      }
    }

    // Reorder Level validation
    if (!this.newProduct.reorderLevel || this.newProduct.reorderLevel < 0) {
      this.reorderLevelError = 'Reorder level is required and cannot be negative';
      hasError = true;
    }

    // Stop if validation fails
    if (hasError) return;

    if (!this.newProduct) return;

    const requestPayload: CreateProductRequest = {
      productName: this.newProduct.productName,
      categoryName: this.newProduct.categoryName,
      brandName: this.newProduct.brandName,
      genericName: this.newProduct.genericName,
      formulationName: this.newProduct.formulationName,
      classificationName: this.newProduct.classificationName,
      typeName: this.newProduct.typeName,
      sheetsPerBox: this.newProduct.sheetsPerBox,
      piecesPerSheet: this.newProduct.piecesPerSheet,
      boxPrice: this.newProduct.boxPrice,
      sheetPrice: this.newProduct.sheetPrice,
      piecePrice: this.newProduct.piecePrice,
      costBoxPrice: this.newProduct.costBoxPrice,
      costSheetPrice: this.newProduct.costSheetPrice,
      costPiecePrice: this.newProduct.costPiecePrice,
      reorderLevel: this.newProduct.reorderLevel,
      unitMeasurement: this.newProduct.unitMeasurement,
      piecePerBox: this.newProduct.piecePerBox,
    };

    this.postService.createProductAsync(requestPayload).subscribe({
      next: res => {
        console.log(`${JSON.stringify(res)}`);
        this.showSuccessAlert = true;
      },
      error: err => {
        console.log(`${err.error}`);

        // Handle specific error messages from backend
        if (err.status === 400) {
          if (typeof err.error === 'string' && err.error.includes('already exists')) {
            this.productNameError = 'Product name already exists';
          } else if (typeof err.error === 'string') {
            this.productNameError = err.error;
          } else {
            this.productNameError = 'Failed to create product';
          }
        } else if (err.status === 409) {
          this.productNameError = 'Product already exists';
        } else if (err.status === 500) {
          this.productNameError = 'Server error occurred';
        } else {
          this.productNameError = 'An error occurred while creating the product';
        }
      }
    });
  }

  clearAllErrors() {
    this.productNameError = '';
    this.categoryNameError = '';
    this.unitMeasurementError = '';
    this.sheetsPerBoxError = '';
    this.piecesPerSheetError = '';
    this.piecePerBoxError = '';
    this.costBoxPriceError = '';
    this.costPiecePriceError = '';
    this.boxPriceError = '';
    this.piecePriceError = '';
    this.reorderLevelError = '';
  }

  closeCreateProductForm() {
    this.clearAllErrors();
    this.closeForm.emit();
  }

  closeSuccessAlert() {
    this.showSuccessAlert = false;
    this.closeCreateProductForm();
  }

  selectUnitOfMeasurement(measurement: string) {
    this.unitMeasurement = measurement;
    // Clear prices when changing unit measurement
    this.newProduct.boxPrice = 0;
    this.newProduct.sheetPrice = 0;
    this.newProduct.sheetsPerBox = 0;
    this.newProduct.piecesPerSheet = 0;
    this.newProduct.piecePerBox = 0;
  }
}
