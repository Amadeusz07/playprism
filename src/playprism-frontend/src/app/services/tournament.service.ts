import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Bracket } from '../models/bracket.model';
import { CanJoin } from '../models/can-join.model';
import { Player } from '../models/player.model';
import { Team } from '../models/team.model';
import { CreateTournament } from '../models/tournaments/create-tournament.model';
import { TournamentListItem } from '../models/tournaments/tournament-list-item.model';
import { TournamentDetails } from '../models/tournaments/tournaments-details.model';

@Injectable({
  providedIn: 'root'
})
export class TournamentService {

  private API_URL = environment.API_URL_TOURNAMENT;

  constructor(private http: HttpClient) { }

  public async getTournaments(disciplineId: string): Promise<TournamentListItem[]> {
    const params = new HttpParams()
      .set('disciplineId', disciplineId);
    return await this.http.get<TournamentListItem[]>(`${this.API_URL}/tournament`, { params }).toPromise();
  }

  public async getTournament(tournamentId: string): Promise<TournamentDetails> {
    return await this.http.get<TournamentDetails>(`${this.API_URL}/tournament/${tournamentId}`).toPromise();
  }

  public async getTournamentBracket(tournamentId: string): Promise<Bracket> {
    return await this.http.get<Bracket>(`${this.API_URL}/tournament/${tournamentId}/bracket`).toPromise();
  }

  public postTournament(model: CreateTournament): Observable<TournamentDetails> {
    return this.http.post<TournamentDetails>(`${this.API_URL}/tournament`, model);
  }

  public async joinTournamentAsTeam(tournamentId: number, team: Team): Promise<any> {
    var participant = {
        tournamentId: tournamentId,
        teamId: team.id,
        name: team.name
    };
    await this.http.post(`${this.API_URL}/tournament/join`, participant ).toPromise();
  
    // TODO: handle error
  }

  public async joinTournamentAsPlayer(tournamentId: number, player: Player): Promise<any> {
    var participant = {
      tournamentId: tournamentId,
      playerId: player.id,
      teamId: 0,
      name: player.name
    };
    await this.http.post(`${this.API_URL}/tournament/join`, participant ).toPromise();
    
    // TODO: handle error
  }

  public async canJoinTournament(tournamentId: string, candidateId: number): Promise<CanJoin> {
    return await this.http.get<CanJoin>(`${this.API_URL}/tournament/${tournamentId}/can-join/${candidateId}`).toPromise();
  }
}
