import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Tournament } from '../models/tournament.model';

@Injectable({
  providedIn: 'root'
})
export class TournamentService {

  private API_URL = environment.API_URL_TOURNAMENT;

  constructor(private http: HttpClient) { }

  public async getTournaments(disciplineId: string): Promise<Tournament[]> {
    const params = new HttpParams()
      .set('disciplineId', disciplineId);
    return await this.http.get<Tournament[]>(`${this.API_URL}/tournament`, { params }).toPromise();
  }
}
