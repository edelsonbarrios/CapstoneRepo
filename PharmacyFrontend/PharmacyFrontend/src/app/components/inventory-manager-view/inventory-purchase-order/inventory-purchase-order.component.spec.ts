import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryPurchaseOrderComponent } from './inventory-purchase-order.component';

describe('InventoryPurchaseOrderComponent', () => {
  let component: InventoryPurchaseOrderComponent;
  let fixture: ComponentFixture<InventoryPurchaseOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryPurchaseOrderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryPurchaseOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
