import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MatchDefinition } from '../models/match-definitions.model';

@Injectable({
  providedIn: 'root'
})
export class MatchDefinitionService {
  private API_URL = environment.API_URL_TOURNAMENT;

  constructor(private http: HttpClient) { }

  public async getMatchDefinitions(tournamentId: string): Promise<MatchDefinition[]> {
    return await this.http.get<MatchDefinition[]>(`${this.API_URL}/tournament/${tournamentId}/matchDefinition`).toPromise();
  }

  public updateMatchDefinition(tournamentId: string, matchDefinitionId: string, matchDefinition: MatchDefinition): Observable<Object> {
    return this.http.put(`${this.API_URL}/tournament/${tournamentId}/matchDefinition/${matchDefinitionId}`, matchDefinition);
  }

}
