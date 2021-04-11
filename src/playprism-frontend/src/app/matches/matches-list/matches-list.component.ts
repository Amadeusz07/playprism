import { Component, OnInit, ViewChild } from '@angular/core';
import { MatAccordion } from '@angular/material/expansion';
import { Match } from 'src/app/models/match.model';
import { MatchService } from 'src/app/services/match.service';

@Component({
  selector: 'app-matches-list',
  templateUrl: './matches-list.component.html',
  styleUrls: ['./matches-list.component.scss']
})
export class MatchesListComponent implements OnInit {
  @ViewChild(MatAccordion) accordion: MatAccordion;
  public matches: Match[];
  public error: string;

  constructor(private matchService: MatchService) { }

  ngOnInit(): void {
    this.getMatches();
  }

  public submit(match: Match): void {
    this.matchService.submitResult(match)
      .subscribe(_ => {
          this.getMatches();
        },
        err => this.error = err.error
      );
  }

  private getMatches() {
    this.matchService.getIncomingMatches().subscribe(
      matches => {
        this.matches = matches;
      },
      err => {
        if (err.status == 404) {
          this.error = "No matches found"
        }
        else {
          this.error = "Error occured"
        }
      }
    );
  }

}
