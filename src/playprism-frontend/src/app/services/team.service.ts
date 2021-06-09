import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Player } from '../models/player.model';
import { TeamPlayerAssignment } from '../models/team-player-assignment.model';
import { Team } from '../models/team.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private API_URL = environment.API_URL_TEAM;
  constructor(private http: HttpClient) { }

  public async getPlayerInfo(): Promise<Player> {
    return this.http.get<Player>(`${this.API_URL}/player`).toPromise();
  }

  public async getMyTeam(): Promise<Team> {
    return this.http.get<Team>(`${this.API_URL}/team/current-team`).toPromise();
  }

  public getMyTeams(): Observable<TeamPlayerAssignment[]> {
    return this.http.get<TeamPlayerAssignment[]>(`${this.API_URL}/team`);
  }

  public sendInvite(teamId: number, username: string): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/team/${teamId}/invite/${username}`, null);
  }

  public joinTeam(teamId: number): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/team/${teamId}/join`, null);
  }

  public leaveTeam(teamId: number): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/team/${teamId}/leave`, null);
  }

  public refuseTeam(teamId: number): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/team/${teamId}/refuse`, null);
  }

  public postTeam(team: Team): Observable<Team> {
    return this.http.post<Team>(`${this.API_URL}/team`, team);
  }

  public putTeam(team: Team): Observable<Team> {
    return this.http.put<Team>(`${this.API_URL}/team/${team.id}`, team);
  }

  public deleteTeam(team: Team): Observable<any> {
    return this.http.delete<Team>(`${this.API_URL}/team/${team.id}`);
  }
}
