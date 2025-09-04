import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryModuleMainComponent } from './inventory-module-main.component';

describe('InventoryModuleMainComponent', () => {
  let component: InventoryModuleMainComponent;
  let fixture: ComponentFixture<InventoryModuleMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryModuleMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryModuleMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
