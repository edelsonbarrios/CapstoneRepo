import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryManagerProductMainComponent } from './inventory-manager-product-main.component';

describe('InventoryManagerProductMainComponent', () => {
  let component: InventoryManagerProductMainComponent;
  let fixture: ComponentFixture<InventoryManagerProductMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryManagerProductMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryManagerProductMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
