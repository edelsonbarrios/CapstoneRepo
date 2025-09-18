import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSettleComponent } from './view-settle.component';

describe('ViewSettleComponent', () => {
  let component: ViewSettleComponent;
  let fixture: ComponentFixture<ViewSettleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewSettleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewSettleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
