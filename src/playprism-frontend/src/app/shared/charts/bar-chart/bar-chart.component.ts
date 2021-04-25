import { Component, Input } from '@angular/core';
import { PlayerRecords } from 'src/app/models/player-records.model';

@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.scss']
})
export class BarChartComponent {
  @Input() data: PlayerRecords[];
  @Input() showXAxis: boolean = true;
  @Input() showYAxis: boolean = true;
  @Input() gradient: boolean = true;
  @Input() showLegend: boolean = true;
  @Input() showXAxisLabel: boolean = false;
  @Input() xAxisLabel: string = 'Discipline';
  @Input() showYAxisLabel: boolean = false;
  @Input() yAxisLabel: string = 'Population';
  @Input() legendTitle: string = 'Your Records';

  public colorScheme = {
    domain: ['#5AA454', '#db371a']
  };

  constructor() {
  }
}
