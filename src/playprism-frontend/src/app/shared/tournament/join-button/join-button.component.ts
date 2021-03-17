import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CanJoin } from 'src/app/models/can-join.model';
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
  @Input() canJoin: CanJoin;
  @Output() clicked: EventEmitter<any> = new EventEmitter();

  public labels: {
    registrationPending: 'Register request sent to the tournament\'s organizer'
  }

  constructor() { }

  ngOnInit(): void {
  }

  public join() {
    this.clicked.emit();
  }

}
