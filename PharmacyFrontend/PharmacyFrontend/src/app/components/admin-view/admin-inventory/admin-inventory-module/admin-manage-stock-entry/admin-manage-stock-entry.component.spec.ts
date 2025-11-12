import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManageStockEntryComponent } from './admin-manage-stock-entry.component';

describe('AdminManageStockEntryComponent', () => {
  let component: AdminManageStockEntryComponent;
  let fixture: ComponentFixture<AdminManageStockEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminManageStockEntryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManageStockEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
