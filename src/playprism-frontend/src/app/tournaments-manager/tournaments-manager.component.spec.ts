import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TournamentsManagerComponent } from './tournaments-manager.component';

describe('TournamentsManagerComponent', () => {
  let component: TournamentsManagerComponent;
  let fixture: ComponentFixture<TournamentsManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TournamentsManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TournamentsManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
