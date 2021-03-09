import { Component, OnInit } from '@angular/core';
import { TeamPlayerAssignment } from '../models/team-player-assignment.model';
import { TeamService } from '../services/team.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit {
  public assignments: TeamPlayerAssignment[];
  public usernameToInvite = '';
  constructor(private teamService: TeamService) { }

  public async ngOnInit(): Promise<any> {
    this.assignments = await this.teamService.getMyTeams();
  }

  public invite(teamId: number): void {
    this.teamService.sendInvite(teamId, this.usernameToInvite);
  }

  public join(teamId: number): void {
    this.teamService.joinTeam(teamId);
  }

  public leave(teamId: number): void {
    this.teamService.leaveTeam(teamId);
  }

  public refuse(teamId: number): void {
    this.teamService.refuseTeam(teamId);
  }

}
