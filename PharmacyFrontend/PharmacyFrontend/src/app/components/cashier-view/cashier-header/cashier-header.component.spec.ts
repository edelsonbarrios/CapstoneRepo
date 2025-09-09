import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashierHeaderComponent } from './cashier-header.component';

describe('CashierHeaderComponent', () => {
  let component: CashierHeaderComponent;
  let fixture: ComponentFixture<CashierHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CashierHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashierHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
