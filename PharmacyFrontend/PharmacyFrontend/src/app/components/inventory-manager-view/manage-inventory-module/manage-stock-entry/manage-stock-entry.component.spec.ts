import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageStockEntryComponent } from './manage-stock-entry.component';

describe('ManageStockEntryComponent', () => {
  let component: ManageStockEntryComponent;
  let fixture: ComponentFixture<ManageStockEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageStockEntryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageStockEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
