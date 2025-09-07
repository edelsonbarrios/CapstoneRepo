import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEmployeeHistoryComponent } from './manage-employee-history.component';

describe('ManageEmployeeHistoryComponent', () => {
  let component: ManageEmployeeHistoryComponent;
  let fixture: ComponentFixture<ManageEmployeeHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageEmployeeHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageEmployeeHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
