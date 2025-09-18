import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditGenericComponent } from './edit-generic.component';

describe('EditGenericComponent', () => {
  let component: EditGenericComponent;
  let fixture: ComponentFixture<EditGenericComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditGenericComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditGenericComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
