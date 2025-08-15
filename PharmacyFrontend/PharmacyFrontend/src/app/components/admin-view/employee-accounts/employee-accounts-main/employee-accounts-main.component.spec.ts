import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeAccountsMainComponent } from './employee-accounts-main.component';

describe('EmployeeAccountsMainComponent', () => {
  let component: EmployeeAccountsMainComponent;
  let fixture: ComponentFixture<EmployeeAccountsMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployeeAccountsMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeAccountsMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
