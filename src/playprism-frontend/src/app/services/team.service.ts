import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TeamPlayerAssignment } from '../models/team-player-assignment.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private API_URL = environment.API_URL_TEAM;
  constructor(private http: HttpClient) { }

  public async getMyTeams(): Promise<TeamPlayerAssignment[]> {
    return await this.http.get<TeamPlayerAssignment[]>(`${this.API_URL}/team`).toPromise();
  }

  public async sendInvite(teamId: number, username: string): Promise<any> {
    // return await this.http.post(`${this.API_URL}/team/${teamId}/invite`, { username: username }).toPromise();
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



}
