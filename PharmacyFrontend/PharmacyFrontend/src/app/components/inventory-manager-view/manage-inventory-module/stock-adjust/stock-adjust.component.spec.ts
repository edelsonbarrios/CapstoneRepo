import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockAdjustComponent } from './stock-adjust.component';

describe('StockAdjustComponent', () => {
  let component: StockAdjustComponent;
  let fixture: ComponentFixture<StockAdjustComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StockAdjustComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockAdjustComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
