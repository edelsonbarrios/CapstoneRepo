import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDisposableComponent } from './manage-disposable.component';

describe('ManageDisposableComponent', () => {
  let component: ManageDisposableComponent;
  let fixture: ComponentFixture<ManageDisposableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageDisposableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageDisposableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
