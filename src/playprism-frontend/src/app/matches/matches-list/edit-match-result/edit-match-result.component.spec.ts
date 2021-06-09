import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMatchResultComponent } from './edit-match-result.component';

describe('EditMatchResultComponent', () => {
  let component: EditMatchResultComponent;
  let fixture: ComponentFixture<EditMatchResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditMatchResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMatchResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
