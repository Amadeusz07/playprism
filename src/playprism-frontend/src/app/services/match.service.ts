import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Match } from '../models/match.model';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private API_URL = environment.API_URL_TOURNAMENT;

  constructor(private http: HttpClient) { }

  public getIncomingMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.API_URL}/match`);
  } 
}
