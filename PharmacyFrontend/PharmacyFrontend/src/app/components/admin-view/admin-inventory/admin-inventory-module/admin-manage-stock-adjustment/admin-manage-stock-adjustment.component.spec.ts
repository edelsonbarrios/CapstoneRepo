import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManageStockAdjustmentComponent } from './admin-manage-stock-adjustment.component';

describe('AdminManageStockAdjustmentComponent', () => {
  let component: AdminManageStockAdjustmentComponent;
  let fixture: ComponentFixture<AdminManageStockAdjustmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminManageStockAdjustmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManageStockAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
