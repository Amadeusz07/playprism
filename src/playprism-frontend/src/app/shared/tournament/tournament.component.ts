import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Bracket, Match, Round } from 'src/app/models/bracket.model';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html',
  styleUrls: ['./tournament.component.scss']
})
export class TournamentComponent implements OnInit {
  bracket2 = [
    <Round>{
      roundDate: new Date(2020, 1, 1),
      matches: [
        <Match>{
          participant1: 'Test1',
          participant2: 'Test2',
          participant1Score: 3,
          participant2Score: 2,
        },
        <Match>{
          participant1: 'Test3',
          participant2: 'Test4',
          participant1Score: 0,
          participant2Score: 1
        },
      ],
    },
    <Round>{
      matches: [
        <Match>{
          participant1: 'TBD',
          participant2: 'TBD',
          participant1Score: null,
          participant2Score: null,
        }
      ]
    }
  ];
  public bracket: Bracket;

  constructor(private tournamentService: TournamentService, private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const tournamentId = this.route.snapshot.paramMap.get('tournamentId');
    if (tournamentId) {
      this.bracket = await this.tournamentService.getTournamentBracket(tournamentId);
    }
  }

}
