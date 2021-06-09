import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { TournamentStatusEnum } from 'src/app/models/enums/tournament-status.enum';
import { TournamentListItem } from 'src/app/models/tournaments/tournament-list-item.model';
import { TournamentStatusPipe } from 'src/app/pipes/tournament-status.pipe';
import { DisciplineService } from 'src/app/services/discipline.service';
import { TournamentService } from 'src/app/services/tournament.service';

const STATUS_ALL = 'All';

@Component({
  selector: 'app-browse-tournaments',
  templateUrl: './browse-tournaments.component.html',
  styleUrls: ['./browse-tournaments.component.scss']
})
export class BrowseTournamentsComponent implements OnInit {
  public searchText: string;
  public searchStatus: string = STATUS_ALL;
  public tournamentStatus = TournamentStatusEnum;
  public keys = Object.keys;
  public tournaments: TournamentListItem[] = [];
  public displayTournaments: TournamentListItem[] = [];
  public disciplineName: string;
  public statusOptions: string[] = [ STATUS_ALL, ...(Object.keys(this.tournamentStatus))];

  constructor(
    private tournamentService: TournamentService, 
    private disciplineService: DisciplineService, 
    private route: ActivatedRoute, 
    private tournamentStatusPipe: TournamentStatusPipe
  ) { }

  async ngOnInit(): Promise<void> {
    const disciplineId = this.route.snapshot.paramMap.get('disciplineId');
    if (disciplineId) {
      this.tournaments = await this.tournamentService.getTournaments(disciplineId);
      this.displayTournaments = [ ...this.tournaments ];
      if (!this.tournaments || this.tournaments.length == 0)
      {
        await this.getDiscipline(disciplineId);
      }
      else
      {
        this.disciplineName = this.tournaments[0].disciplineName;
      }
    }
  }

  private async getDiscipline(disciplineId: string) {
    const disciplines = await this.disciplineService.getDisciplines();
    const discipline = disciplines.find(d => d.id.toString() === disciplineId);
    if (discipline)
      this.disciplineName = discipline.name;
  }

  public search(): void {
    let displayList = this.searchByName()
    displayList = this.searchByStatus(displayList);
    this.displayTournaments = displayList;
  }

  private searchByName(): TournamentListItem[] {
    if (!this.searchText) {
      return [ ...this.tournaments ];
    }
    return this.tournaments.filter(element => {
      return element.name.toLowerCase().includes(this.searchText.toLowerCase());
    })
  }

  private searchByStatus(list: TournamentListItem[]): TournamentListItem[] {
    if (this.searchStatus == STATUS_ALL) {
      return list;
    }
    return list.filter(element => {
      const tournamentStatus = this.tournamentStatusPipe.transform(element);
      return tournamentStatus.toLowerCase() == this.searchStatus.toLowerCase();
    })
  }

}
