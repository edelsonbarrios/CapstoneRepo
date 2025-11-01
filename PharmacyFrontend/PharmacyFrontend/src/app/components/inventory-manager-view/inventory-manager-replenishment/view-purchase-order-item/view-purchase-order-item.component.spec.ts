import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPurchaseOrderItemComponent } from './view-purchase-order-item.component';

describe('ViewPurchaseOrderItemComponent', () => {
  let component: ViewPurchaseOrderItemComponent;
  let fixture: ComponentFixture<ViewPurchaseOrderItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewPurchaseOrderItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewPurchaseOrderItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
