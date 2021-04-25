import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Discipline } from '../models/discipline.model';
import { TournamentStatusEnum } from '../models/enums/tournament-status.enum';
import { TournamentListItem } from '../models/tournaments/tournament-list-item.model';
import { DisciplineService } from '../services/discipline.service';
import { TournamentService } from '../services/tournament.service';
import { CreateTournamentDialogComponent } from './create-tournament-dialog/create-tournament-dialog.component';

@Component({
  selector: 'app-tournaments-manager',
  templateUrl: './tournaments-manager.component.html',
  styleUrls: ['./tournaments-manager.component.scss']
})
export class TournamentsManagerComponent implements OnInit {
  displayedColumns: string[] = ['name', 'disciplineName', 'startDate', 'participants', 'status', 'actions'];
  public disciplines: Discipline[];
  public tournaments: TournamentListItem[] | null;
  public tournamentStatus = TournamentStatusEnum;
  public error: string | null;

  constructor(private tournamentService: TournamentService, private disciplineService: DisciplineService, public dialog: MatDialog) { }

  async ngOnInit(): Promise<void> {
    await this.loadTournaments();
    this.disciplines = await this.disciplineService.getDisciplines();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CreateTournamentDialogComponent, {
      width: '35%',
      data: { disciplines: this.disciplines }
    });
  }

  public async deleteTournament(tournamentId: number): Promise<void> {
    this.tournamentService.deleteTournament(tournamentId.toString())
      .subscribe(
        _ => this.loadTournaments(),
        err => this.error = 'Error occured during deleting Tournament'
      );
  }

  private async loadTournaments(): Promise<void> {
    this.tournaments = await this.tournamentService.getMyTournaments();
  }

}
