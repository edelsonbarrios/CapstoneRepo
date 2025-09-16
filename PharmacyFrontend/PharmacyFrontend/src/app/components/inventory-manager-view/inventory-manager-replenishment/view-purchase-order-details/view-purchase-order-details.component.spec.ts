import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPurchaseOrderDetailsComponent } from './view-purchase-order-details.component';

describe('ViewPurchaseOrderDetailsComponent', () => {
  let component: ViewPurchaseOrderDetailsComponent;
  let fixture: ComponentFixture<ViewPurchaseOrderDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewPurchaseOrderDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewPurchaseOrderDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
