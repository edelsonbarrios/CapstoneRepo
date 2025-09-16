import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageReplenishmentComponent } from './manage-replenishment.component';

describe('ManageReplenishmentComponent', () => {
  let component: ManageReplenishmentComponent;
  let fixture: ComponentFixture<ManageReplenishmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageReplenishmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageReplenishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
