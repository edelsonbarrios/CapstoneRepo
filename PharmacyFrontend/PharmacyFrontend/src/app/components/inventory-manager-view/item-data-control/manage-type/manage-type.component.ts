import { Component, OnInit, Type } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddTypeComponent } from '../add-type/add-type.component';
import { EditTypeComponent } from '../edit-type/edit-type.component';
import { TypeModel } from '../../../../models/type-models/type';
import { GetService } from '../../../../services/GetService/get.service';
import { NgForOf } from "../../../../../../node_modules/@angular/common/index";
@Component({
  selector: 'app-manage-type',
  standalone: true,
  imports: [NgIf, AddTypeComponent, EditTypeComponent, NgFor],
  templateUrl: './manage-type.component.html',
  styleUrl: './manage-type.component.scss'
})
export class ManageTypeComponent implements OnInit{
  addType ?: TypeModel | null;
  editType ?: TypeModel | null;
  types : TypeModel[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllTypes();
  }

  getAllTypes() {
    this.getService.getAllTypesAsync().subscribe({
      next: res => {
        this.types = res;
      },
      error : err => {
        console.log(`${err.error}`);
      }
    });
  }

  openAddTypeForm() {
    this.addType = new TypeModel();
  }

  closeAddTypeForm() {
    this.addType = null;
    this.getAllTypes();
  }

  openEditTypeForm(type : TypeModel) {
    this.editType = type;
  }

  closeEditTypeForm() {
    this.editType = null;
  }
}
