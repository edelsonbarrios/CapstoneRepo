import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDisposableComponent } from './admin-disposable.component';

describe('AdminDisposableComponent', () => {
  let component: AdminDisposableComponent;
  let fixture: ComponentFixture<AdminDisposableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminDisposableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDisposableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
