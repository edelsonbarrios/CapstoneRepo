import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManageCategoryComponent } from './admin-manage-category.component';

describe('AdminManageCategoryComponent', () => {
  let component: AdminManageCategoryComponent;
  let fixture: ComponentFixture<AdminManageCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminManageCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManageCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
