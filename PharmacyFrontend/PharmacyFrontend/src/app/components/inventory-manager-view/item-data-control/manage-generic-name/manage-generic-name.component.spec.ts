import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageGenericNameComponent } from './manage-generic-name.component';

describe('ManageGenericNameComponent', () => {
  let component: ManageGenericNameComponent;
  let fixture: ComponentFixture<ManageGenericNameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageGenericNameComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageGenericNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
