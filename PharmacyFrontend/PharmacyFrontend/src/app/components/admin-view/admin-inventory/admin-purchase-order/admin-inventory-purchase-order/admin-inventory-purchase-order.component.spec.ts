import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminInventoryPurchaseOrderComponent } from './admin-inventory-purchase-order.component';

describe('AdminInventoryPurchaseOrderComponent', () => {
  let component: AdminInventoryPurchaseOrderComponent;
  let fixture: ComponentFixture<AdminInventoryPurchaseOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminInventoryPurchaseOrderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminInventoryPurchaseOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
