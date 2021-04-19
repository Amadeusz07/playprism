import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PlayerRecords } from '../models/player-records.model';

@Injectable({
  providedIn: 'root'
})
export class PlayerStatisticsService {

  private API_URL = environment.API_URL_TOURNAMENT;
  
  constructor(private http: HttpClient) { }

  public getPlayerStatistics(): Observable<PlayerRecords[]> {
    return this.http.get<PlayerRecords[]>(`${this.API_URL}/statistics/player`);
  }

}
