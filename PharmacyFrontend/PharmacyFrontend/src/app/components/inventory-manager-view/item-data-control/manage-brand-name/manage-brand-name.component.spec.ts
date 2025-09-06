import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBrandNameComponent } from './manage-brand-name.component';

describe('ManageBrandNameComponent', () => {
  let component: ManageBrandNameComponent;
  let fixture: ComponentFixture<ManageBrandNameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageBrandNameComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageBrandNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
