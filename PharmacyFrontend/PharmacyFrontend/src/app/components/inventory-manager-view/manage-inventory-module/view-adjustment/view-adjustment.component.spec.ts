import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAdjustmentComponent } from './view-adjustment.component';

describe('ViewAdjustmentComponent', () => {
  let component: ViewAdjustmentComponent;
  let fixture: ComponentFixture<ViewAdjustmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewAdjustmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
