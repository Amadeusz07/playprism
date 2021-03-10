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
    await this.getAssignments();
  }

  public async invite(teamId: number): Promise<void> {
    await this.teamService.sendInvite(teamId, this.usernameToInvite);
    await this.getAssignments();
  }

  public async join(teamId: number): Promise<void> {
    await this.teamService.joinTeam(teamId);
    await this.getAssignments();
  }

  public async leave(teamId: number): Promise<void> {
    await this.teamService.leaveTeam(teamId);
    await this.getAssignments();
  }

  public async refuse(teamId: number): Promise<void> {
    this.teamService.refuseTeam(teamId);
    await this.getAssignments();
  }

  public async getAssignments() {
    this.assignments = await this.teamService.getMyTeams();
  }

}
