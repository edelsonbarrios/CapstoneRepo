import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { AddFormulationComponent } from '../add-formulation/add-formulation.component';
import { EditFormulationComponent } from '../edit-formulation/edit-formulation.component';
import { GetService } from '../../../../services/GetService/get.service';
import { Formulation } from '../../../../models/formulation-models/formulation';

@Component({
  selector: 'app-manage-formulation',
  standalone: true,
  imports: [NgIf,AddFormulationComponent,EditFormulationComponent, NgFor],
  templateUrl: './manage-formulation.component.html',
  styleUrl: './manage-formulation.component.scss'
})
export class ManageFormulationComponent implements OnInit{
  addFormulation ?: Formulation | null;
  editFormulation ?: Formulation | null;
  formulations: Formulation[] = [];

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllFormulation();
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

  openAddFormulationForm() {
    this.addFormulation = new Formulation();
  }

  closeAddFormulationForm() {
    this.addFormulation = null;
    this.getAllFormulation();
  }

  openEditFormulationForm(formulation : Formulation) {
    this.editFormulation = formulation;
  }

  closeEditFormulationForm() {
    this.editFormulation = null;
  }
}
