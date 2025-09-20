import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFormulationComponent } from './edit-formulation.component';

describe('EditFormulationComponent', () => {
  let component: EditFormulationComponent;
  let fixture: ComponentFixture<EditFormulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditFormulationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFormulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
