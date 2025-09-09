import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashierMainComponent } from './cashier-main.component';

describe('CashierMainComponent', () => {
  let component: CashierMainComponent;
  let fixture: ComponentFixture<CashierMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CashierMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashierMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
