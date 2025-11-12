import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManageProductComponent } from './admin-manage-product.component';

describe('AdminManageProductComponent', () => {
  let component: AdminManageProductComponent;
  let fixture: ComponentFixture<AdminManageProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminManageProductComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManageProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
