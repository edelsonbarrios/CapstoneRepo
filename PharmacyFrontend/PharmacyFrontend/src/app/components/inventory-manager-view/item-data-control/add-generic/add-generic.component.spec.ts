import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGenericComponent } from './add-generic.component';

describe('AddGenericComponent', () => {
  let component: AddGenericComponent;
  let fixture: ComponentFixture<AddGenericComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddGenericComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddGenericComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
