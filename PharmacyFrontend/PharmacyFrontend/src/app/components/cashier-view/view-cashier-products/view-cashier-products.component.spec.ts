import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCashierProductsComponent } from './view-cashier-products.component';

describe('ViewCashierProductsComponent', () => {
  let component: ViewCashierProductsComponent;
  let fixture: ComponentFixture<ViewCashierProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewCashierProductsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewCashierProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
