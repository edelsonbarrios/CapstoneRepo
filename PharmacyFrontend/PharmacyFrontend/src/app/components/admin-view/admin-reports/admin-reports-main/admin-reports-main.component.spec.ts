import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminReportsMainComponent } from './admin-reports-main.component';

describe('AdminReportsMainComponent', () => {
  let component: AdminReportsMainComponent;
  let fixture: ComponentFixture<AdminReportsMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminReportsMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminReportsMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
