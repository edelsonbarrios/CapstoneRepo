import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminInventoryModuleMainComponent } from './admin-inventory-module-main.component';

describe('AdminInventoryModuleMainComponent', () => {
  let component: AdminInventoryModuleMainComponent;
  let fixture: ComponentFixture<AdminInventoryModuleMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminInventoryModuleMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminInventoryModuleMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
