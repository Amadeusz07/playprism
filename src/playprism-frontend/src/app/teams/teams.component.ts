import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TeamPlayerAssignment } from '../models/team-player-assignment.model';
import { Team } from '../models/team.model';
import { TeamService } from '../services/team.service';
import { TeamDialogComponent } from './team-dialog/team-dialog.component';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit {
  public assignments: TeamPlayerAssignment[] = [];
  public usernameToInvite = '';
  constructor(private teamService: TeamService, public dialog: MatDialog) { }

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
  
  public openCreateDialog(): void {
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width: '35%',
      data: { isEdit: false }
    });
    dialogRef.afterClosed().subscribe(_ => this.getAssignments());
  }

  public openEditDialog(team: Team ): void {
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width: '35%',
      data: { team: team, isEdit: true }
    });
    dialogRef.afterClosed().subscribe(_ => this.getAssignments());
  }
}
