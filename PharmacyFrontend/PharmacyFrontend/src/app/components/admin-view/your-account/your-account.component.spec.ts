import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YourAccountComponent } from './your-account.component';

describe('YourAccountComponent', () => {
  let component: YourAccountComponent;
  let fixture: ComponentFixture<YourAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YourAccountComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YourAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
