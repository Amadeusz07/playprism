import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { Tournament } from 'src/app/models/tournament.model';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-browse-tournaments',
  templateUrl: './browse-tournaments.component.html',
  styleUrls: ['./browse-tournaments.component.scss']
})
export class BrowseTournamentsComponent implements OnInit {

  public tournaments: Tournament[] = [];
  public tournamentFormat = TournamentFormatEnum;

  constructor(private tournamentService: TournamentService, private router: Router, private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const disciplineId = this.route.snapshot.paramMap.get('disciplineId');
    if (disciplineId) {
      this.tournaments = await this.tournamentService.getTournaments(disciplineId);
    }
  }

}
