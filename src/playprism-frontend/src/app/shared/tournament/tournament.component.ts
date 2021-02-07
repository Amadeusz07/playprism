import { Component, OnInit } from '@angular/core';
import { Match, Round } from 'src/app/models/bracket.model';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html',
  styleUrls: ['./tournament.component.scss']
})
export class TournamentComponent implements OnInit {
  bracket = [
    <Round>{
      matches: [
        <Match>{
          participant1: 'Test1',
          participant2: 'Test2',
          participantScore1: 3,
          participantScore2: 2,
        },
        <Match>{
          participant1: 'Test3',
          participant2: 'Test4',
          participantScore1: 0,
          participantScore2: 1
        },
      ],
    },
    <Round>{
      nextRound: <Round>{},
      matches: [
        <Match>{
          participant1: 'TBD',
          participant2: 'TBD',
          participantScore1: null,
          participantScore2: null,
        }
      ]
    }
  ];
  constructor() { }

  ngOnInit(): void {
  }

}
