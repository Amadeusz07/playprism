import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigureTournamentComponent } from './configure-tournament.component';

describe('ConfigureTournamentComponent', () => {
  let component: ConfigureTournamentComponent;
  let fixture: ComponentFixture<ConfigureTournamentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigureTournamentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigureTournamentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
