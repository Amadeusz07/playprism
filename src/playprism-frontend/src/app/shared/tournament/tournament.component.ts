import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Bracket } from 'src/app/models/bracket.model';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { TournamentStatusEnum } from 'src/app/models/enums/tournament-status.enum';
import { TournamentDetails } from 'src/app/models/tournaments/tournaments-details.model';
import { TournamentStatusPipe } from 'src/app/pipes/tournament-status.pipe';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html',
  styleUrls: ['./tournament.component.scss']
})
export class TournamentComponent implements OnInit {
  public tournamentFormatEnum = TournamentFormatEnum;
  public bracket: Bracket;
  public tournament: TournamentDetails;
  public get tournamentStatus(): TournamentStatusEnum {
    return this.statusPipe.transform(this.tournament);
  }
  public get tournamentFormatString(): string {
    switch(this.tournament.format) {
      case TournamentFormatEnum.SingleElimination: {
        return 'Single Elimination';
      }
      case TournamentFormatEnum.League: {
        return 'League';
      }
      case TournamentFormatEnum.Groups: {
        return 'Groups';
      }
    }
  }

  constructor(private tournamentService: TournamentService, private statusPipe: TournamentStatusPipe, private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const tournamentId = this.route.snapshot.paramMap.get('tournamentId');
    if (tournamentId) {
      this.bracket = await this.tournamentService.getTournamentBracket(tournamentId);
      this.tournament = await this.tournamentService.getTournament(tournamentId);
    }
  }

  public join(): void {

  }

}
