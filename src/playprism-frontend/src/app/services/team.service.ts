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
    return await this.http.get<Player>(`${this.API_URL}/player`).toPromise();
  }

  public async getMyTeam(): Promise<Team> {
    return await this.http.get<Team>(`${this.API_URL}/team/current-team`).toPromise();
  }

  public async getMyTeams(): Promise<TeamPlayerAssignment[]> {
    return await this.http.get<TeamPlayerAssignment[]>(`${this.API_URL}/team`).toPromise();
  }

  public async sendInvite(teamId: number, username: string): Promise<any> {
    return await this.http.post(`${this.API_URL}/team/${teamId}/invite/${username}`, null).toPromise();
  }

  public async joinTeam(teamId: number): Promise<any> {
    return await this.http.post(`${this.API_URL}/team/${teamId}/join`, null).toPromise();
  }

  public async leaveTeam(teamId: number): Promise<any> {
    return await this.http.post(`${this.API_URL}/team/${teamId}/leave`, null).toPromise();
  }

  public async refuseTeam(teamId: number): Promise<any> {
    return await this.http.post(`${this.API_URL}/team/${teamId}/refuse`, null).toPromise();
  }

  public postTeam(team: Team): Observable<Team> {
    return this.http.post<Team>(`${this.API_URL}/team`, team);
  }



}
