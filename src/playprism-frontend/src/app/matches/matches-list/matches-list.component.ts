import { Component, OnInit } from '@angular/core';
import { Match } from 'src/app/models/match.model';
import { MatchService } from 'src/app/services/match.service';

@Component({
  selector: 'app-matches-list',
  templateUrl: './matches-list.component.html',
  styleUrls: ['./matches-list.component.scss']
})
export class MatchesListComponent implements OnInit {
  public matches: Match[];
  public error: string;
  constructor(private matchService: MatchService) { }

  ngOnInit(): void {
    this.matchService.getIncomingMatches().subscribe(
      matches => this.matches = matches, 
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
