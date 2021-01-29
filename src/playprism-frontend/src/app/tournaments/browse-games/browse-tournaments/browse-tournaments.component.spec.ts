import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowseTournamentsComponent } from './browse-tournaments.component';

describe('BrowseTournamentsComponent', () => {
  let component: BrowseTournamentsComponent;
  let fixture: ComponentFixture<BrowseTournamentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BrowseTournamentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowseTournamentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
