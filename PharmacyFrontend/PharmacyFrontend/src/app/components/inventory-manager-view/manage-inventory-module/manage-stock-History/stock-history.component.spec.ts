import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageStockHistoryComponent} from './stock-history.component';

describe('StockHistoryComponent', () => {
  let component: ManageStockHistoryComponent;
  let fixture: ComponentFixture<ManageStockHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageStockHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageStockHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});