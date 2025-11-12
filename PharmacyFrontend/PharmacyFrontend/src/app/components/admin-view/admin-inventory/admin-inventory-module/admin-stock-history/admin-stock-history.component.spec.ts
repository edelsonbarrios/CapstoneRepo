import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminStockHistoryComponent } from './admin-stock-history.component';

describe('AdminStockHistoryComponent', () => {
  let component: AdminStockHistoryComponent;
  let fixture: ComponentFixture<AdminStockHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminStockHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminStockHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
