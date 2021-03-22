import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Discipline } from '../models/discipline.model';
import { DisciplineService } from '../services/discipline.service';
import { CreateTournamentDialogComponent } from './create-tournament-dialog/create-tournament-dialog.component';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  { position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H' },
  { position: 2, name: 'Helium', weight: 4.0026, symbol: 'He' },
  { position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li' },
  { position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be' },
  { position: 5, name: 'Boron', weight: 10.811, symbol: 'B' },
  { position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C' },
  { position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N' },
  { position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O' },
  { position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F' },
  { position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne' },
];

@Component({
  selector: 'app-tournaments-manager',
  templateUrl: './tournaments-manager.component.html',
  styleUrls: ['./tournaments-manager.component.scss']
})
export class TournamentsManagerComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  dataSource = ELEMENT_DATA;
  public disciplines: Discipline[];

  constructor(private disciplineService: DisciplineService, public dialog: MatDialog) { }

  async ngOnInit(): Promise<void> {
    this.disciplines = await this.disciplineService.getDisciplines();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CreateTournamentDialogComponent, {
      width: '35%',
      data: { disciplines: this.disciplines }
    });

    // dialogRef.afterClosed().subscribe(result => {
    //   console.log('The dialog was closed');
    //   // this.animal = result;
    // });
  }

}
