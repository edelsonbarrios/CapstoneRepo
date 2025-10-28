import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RestoreConfirmationComponent } from './restore-confirmation.component';

describe('RestoreConfirmationComponent', () => {
  let component: RestoreConfirmationComponent;
  let fixture: ComponentFixture<RestoreConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RestoreConfirmationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RestoreConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
