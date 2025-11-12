import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProductMainComponent } from './admin-product-main.component';

describe('AdminProductMainComponent', () => {
  let component: AdminProductMainComponent;
  let fixture: ComponentFixture<AdminProductMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminProductMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminProductMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
