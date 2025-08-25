import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMaintenanceMainComponent } from './admin-maintenance-main.component';

describe('AdminMaintenanceMainComponent', () => {
  let component: AdminMaintenanceMainComponent;
  let fixture: ComponentFixture<AdminMaintenanceMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMaintenanceMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminMaintenanceMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
