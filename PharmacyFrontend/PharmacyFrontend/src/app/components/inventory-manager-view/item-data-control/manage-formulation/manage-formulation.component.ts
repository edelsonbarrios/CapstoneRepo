import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { Product } from '../../../../models/product-models/product';
import { AddFormulationComponent } from '../add-formulation/add-formulation.component';
import { EditFormulationComponent } from '../edit-formulation/edit-formulation.component';

@Component({
  selector: 'app-manage-formulation',
  standalone: true,
  imports: [NgIf,AddFormulationComponent,EditFormulationComponent],
  templateUrl: './manage-formulation.component.html',
  styleUrl: './manage-formulation.component.scss'
})
export class ManageFormulationComponent {
  addFormulation ?: Product | null;
  editFormulation ?: Product | null;

  openAddFormulationForm() {
    this.addFormulation = new Product();
  }

  openEditFormulationForm() {
    this.editFormulation = new Product();
  }
}
