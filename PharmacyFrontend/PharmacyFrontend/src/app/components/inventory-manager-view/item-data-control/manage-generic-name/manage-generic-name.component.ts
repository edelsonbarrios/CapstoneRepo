import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { NgFor, NgIf } from '@angular/common';
import { AddGenericComponent } from '../add-generic/add-generic.component';
import { EditGenericComponent } from '../edit-generic/edit-generic.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Generic } from '../../../../models/generic-models/generic';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-generic-name',
  standalone: true,
  imports: [
    NgIf,
    AddGenericComponent,
    EditGenericComponent,
    FormsModule,
    NgFor,
  ],
  templateUrl: './manage-generic-name.component.html',
  styleUrl: './manage-generic-name.component.scss',
})
export class ManageGenericNameComponent implements OnInit {
  addGeneric?: Generic | null;
  editGeneric?: Generic | null;
  generics: Generic[] = [];

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getAllGenerics();
  }

  getAllGenerics() {
    this.getService.getAllGenericsAsync().subscribe({
      next: (res) => {
        this.generics = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  openAddGenericForm() {
    this.addGeneric = new Product();
  }

  closeAddGenericForm() {
    this.addGeneric = null;
    this.getAllGenerics();
  }

  openEditGenericForm() {
    this.editGeneric = new Product();
  }
}
