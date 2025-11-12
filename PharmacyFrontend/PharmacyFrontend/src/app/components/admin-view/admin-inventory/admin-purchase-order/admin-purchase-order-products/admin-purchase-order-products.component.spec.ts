import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPurchaseOrderProductsComponent } from './admin-purchase-order-products.component';

describe('AdminPurchaseOrderProductsComponent', () => {
  let component: AdminPurchaseOrderProductsComponent;
  let fixture: ComponentFixture<AdminPurchaseOrderProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminPurchaseOrderProductsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPurchaseOrderProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
