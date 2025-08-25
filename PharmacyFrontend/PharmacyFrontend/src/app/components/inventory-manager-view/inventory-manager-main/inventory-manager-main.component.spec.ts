import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryManagerMainComponent } from './inventory-manager-main.component';

describe('InventoryManagerMainComponent', () => {
  let component: InventoryManagerMainComponent;
  let fixture: ComponentFixture<InventoryManagerMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryManagerMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryManagerMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
