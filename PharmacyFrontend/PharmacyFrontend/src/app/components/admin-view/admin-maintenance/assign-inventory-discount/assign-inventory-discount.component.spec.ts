import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignInventoryDiscountComponent } from './assign-inventory-discount.component';

describe('AssignInventoryDiscountComponent', () => {
  let component: AssignInventoryDiscountComponent;
  let fixture: ComponentFixture<AssignInventoryDiscountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignInventoryDiscountComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignInventoryDiscountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
