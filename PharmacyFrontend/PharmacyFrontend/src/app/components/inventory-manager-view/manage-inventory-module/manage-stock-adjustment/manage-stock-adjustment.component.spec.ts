import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageStockAdjustmentComponent } from './manage-stock-adjustment.component';

describe('ManageStockAdjustmentComponent', () => {
  let component: ManageStockAdjustmentComponent;
  let fixture: ComponentFixture<ManageStockAdjustmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageStockAdjustmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageStockAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
