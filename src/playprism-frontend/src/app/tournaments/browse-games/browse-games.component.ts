import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Discipline } from 'src/app/models/discipline.model';
import { DisciplineService } from 'src/app/services/discipline.service';

@Component({
  selector: 'app-browse-games',
  templateUrl: './browse-games.component.html',
  styleUrls: ['./browse-games.component.scss']
})
export class BrowseGamesComponent implements OnInit {
  public searchText: string;
  public displayDiscplines: Discipline[];
  private disciplines: Discipline[] = [];
  constructor(private disciplineService: DisciplineService) {}

  async ngOnInit(): Promise<void> {
    this.disciplines = await this.disciplineService.getDisciplines();
    this.displayDiscplines = [ ...this.disciplines ];
  }

  public search(text: string): void {
    this.displayDiscplines = this.disciplines.filter(element => {
      return element.name.toLowerCase().includes(text.toLowerCase());
    })
  }

}
