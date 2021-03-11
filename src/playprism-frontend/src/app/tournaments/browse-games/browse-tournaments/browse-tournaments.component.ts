import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { TournamentListItem } from 'src/app/models/tournaments/tournament-list-item.model';
import { DisciplineService } from 'src/app/services/discipline.service';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-browse-tournaments',
  templateUrl: './browse-tournaments.component.html',
  styleUrls: ['./browse-tournaments.component.scss']
})
export class BrowseTournamentsComponent implements OnInit {

  public tournaments: TournamentListItem[] = [];
  public tournamentFormat = TournamentFormatEnum;
  public disciplineName: string;

  constructor(private tournamentService: TournamentService, private disciplineService: DisciplineService, private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const disciplineId = this.route.snapshot.paramMap.get('disciplineId');
    if (disciplineId) {
      this.tournaments = await this.tournamentService.getTournaments(disciplineId);
      if (!this.tournaments || this.tournaments.length == 0)
      {
        const disciplines = await this.disciplineService.getDisciplines();
        const discipline = disciplines.find(d => d.id.toString() === disciplineId);
        if (discipline) this.disciplineName = discipline.name;
      }
      else
      {
        this.disciplineName = this.tournaments[0].disciplineName;
      }
    }
  }

}
