import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTournamentDialogComponent } from './create-tournament-dialog.component';

describe('CreateTournamentDialogComponent', () => {
  let component: CreateTournamentDialogComponent;
  let fixture: ComponentFixture<CreateTournamentDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateTournamentDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateTournamentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
