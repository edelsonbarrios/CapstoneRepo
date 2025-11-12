import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManageReplenishmentComponent } from './admin-manage-replenishment.component';

describe('AdminManageReplenishmentComponent', () => {
  let component: AdminManageReplenishmentComponent;
  let fixture: ComponentFixture<AdminManageReplenishmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminManageReplenishmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManageReplenishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
