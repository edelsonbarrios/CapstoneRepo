import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignProductsSupplierComponent } from './assign-products-supplier.component';

describe('AssignProductsSupplierComponent', () => {
  let component: AssignProductsSupplierComponent;
  let fixture: ComponentFixture<AssignProductsSupplierComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignProductsSupplierComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignProductsSupplierComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
