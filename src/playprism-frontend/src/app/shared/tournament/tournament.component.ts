import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Bracket, Match, Round } from 'src/app/models/bracket.model';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { Tournament } from 'src/app/models/tournament.model';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html',
  styleUrls: ['./tournament.component.scss']
})
export class TournamentComponent implements OnInit {
  public tournamentFormat = TournamentFormatEnum;
  public bracket: Bracket;
  public tournament: Tournament;

  constructor(private tournamentService: TournamentService, private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const tournamentId = this.route.snapshot.paramMap.get('tournamentId');
    if (tournamentId) {
      this.bracket = await this.tournamentService.getTournamentBracket(tournamentId);
      this.tournament = await this.tournamentService.getTournament(tournamentId);
    }
  }

}
