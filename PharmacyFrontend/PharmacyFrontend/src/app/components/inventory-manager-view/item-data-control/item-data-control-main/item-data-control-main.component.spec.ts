import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemDataControlMainComponent } from './item-data-control-main.component';

describe('ItemDataControlMainComponent', () => {
  let component: ItemDataControlMainComponent;
  let fixture: ComponentFixture<ItemDataControlMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemDataControlMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemDataControlMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
