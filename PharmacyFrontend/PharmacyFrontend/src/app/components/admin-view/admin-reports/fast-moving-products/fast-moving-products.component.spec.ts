import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FastMovingProductsComponent } from './fast-moving-products.component';

describe('FastMovingProductsComponent', () => {
  let component: FastMovingProductsComponent;
  let fixture: ComponentFixture<FastMovingProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FastMovingProductsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FastMovingProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
