import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashierTransactionComponent } from './cashier-transaction.component';

describe('CashierTransactionComponent', () => {
  let component: CashierTransactionComponent;
  let fixture: ComponentFixture<CashierTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CashierTransactionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashierTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
