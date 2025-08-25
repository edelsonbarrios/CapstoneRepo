import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VatEntryComponent } from './vat-entry.component';

describe('VatEntryComponent', () => {
  let component: VatEntryComponent;
  let fixture: ComponentFixture<VatEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VatEntryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VatEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
