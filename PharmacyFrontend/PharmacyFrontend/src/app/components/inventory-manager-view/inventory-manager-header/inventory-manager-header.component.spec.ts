import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryManagerHeaderComponent } from './inventory-manager-header.component';

describe('InventoryManagerHeaderComponent', () => {
  let component: InventoryManagerHeaderComponent;
  let fixture: ComponentFixture<InventoryManagerHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryManagerHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryManagerHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
