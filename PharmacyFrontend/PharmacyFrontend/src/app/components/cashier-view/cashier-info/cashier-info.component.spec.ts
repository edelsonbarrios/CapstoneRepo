import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashierInfoComponent } from './cashier-info.component';

describe('CashierInfoComponent', () => {
  let component: CashierInfoComponent;
  let fixture: ComponentFixture<CashierInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CashierInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashierInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
