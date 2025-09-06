import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFormulationComponent } from './manage-formulation.component';

describe('ManageFormulationComponent', () => {
  let component: ManageFormulationComponent;
  let fixture: ComponentFixture<ManageFormulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageFormulationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageFormulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
