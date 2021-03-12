import { Component, Input, OnInit } from '@angular/core';
import { TournamentStatusEnum } from 'src/app/models/enums/tournament-status.enum';

@Component({
  selector: 'app-join-button',
  templateUrl: './join-button.component.html',
  styleUrls: ['./join-button.component.scss']
})
export class JoinButtonComponent implements OnInit {
  public tournamentStatusEnum = TournamentStatusEnum;

  @Input() areTeams: boolean;
  @Input() tournamentStatus: TournamentStatusEnum;

  public labels: {
    registrationPending: 'Register request sent to the tournament\'s organizer',
    teamAdded: 'Your team has been added to the tournament',
    playerAdded: 'You have been added to the tournament'
  }

  constructor() { }

  ngOnInit(): void {
  }

  public join() {
    return;
  }

}
