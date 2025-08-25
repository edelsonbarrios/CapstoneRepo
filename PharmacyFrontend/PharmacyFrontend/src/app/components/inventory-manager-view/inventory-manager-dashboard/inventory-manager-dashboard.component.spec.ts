import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryManagerDashboardComponent } from './inventory-manager-dashboard.component';

describe('InventoryManagerDashboardComponent', () => {
  let component: InventoryManagerDashboardComponent;
  let fixture: ComponentFixture<InventoryManagerDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InventoryManagerDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryManagerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
