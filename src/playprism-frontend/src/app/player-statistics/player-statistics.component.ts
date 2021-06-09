import { Component, OnInit } from '@angular/core';
import { PlayerRecords } from '../models/player-records.model';
import { PlayerStatisticsService } from '../services/player-statistics.service';

@Component({
  selector: 'app-player-statistics',
  templateUrl: './player-statistics.component.html',
  styleUrls: ['./player-statistics.component.scss']
})
export class PlayerStatisticsComponent implements OnInit {
  public statisticsData: PlayerRecords[];
  public error: string;

  constructor(private playerStatisticsService: PlayerStatisticsService) { }

  ngOnInit(): void {
    this.playerStatisticsService.getPlayerStatistics().subscribe(
      response => this.statisticsData = response,
      err => this.error = err.error
    );
  }

}
