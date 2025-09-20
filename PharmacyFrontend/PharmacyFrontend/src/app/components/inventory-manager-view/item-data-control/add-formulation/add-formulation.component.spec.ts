import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFormulationComponent } from './add-formulation.component';

describe('AddFormulationComponent', () => {
  let component: AddFormulationComponent;
  let fixture: ComponentFixture<AddFormulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFormulationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddFormulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
