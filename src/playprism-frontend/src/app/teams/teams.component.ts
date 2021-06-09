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
  public error: string | null;
  constructor(private teamService: TeamService, public dialog: MatDialog) { }

  public ngOnInit(): any {
    this.getAssignments();
  }

  public join(teamId: number): void {
    this.error = null
    this.teamService.joinTeam(teamId).subscribe(
      _ => this.getAssignments(),
      err => this.handleError(err)
    );
  }

  public leave(teamId: number): void {
    this.error = null
    this.teamService.leaveTeam(teamId).subscribe(
      _ => this.getAssignments(),
      err => this.handleError(err)
    )
  }

  public refuse(teamId: number): void {
    this.error = null
    this.teamService.refuseTeam(teamId).subscribe(
      _ => this.getAssignments(),
      err => this.handleError(err)
    );
  }

  public getAssignments() {
    this.teamService.getMyTeams().subscribe(assignments => 
      this.assignments = assignments, 
      err => this.handleError(err)
    );
  }
  
  public openCreateDialog(): void {
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width: '35%',
      data: { isEdit: false, isOwner: false }
    });
    dialogRef.afterClosed().subscribe(_ => this.getAssignments());
  }

  public openEditDialog(team: Team ): void {
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width: '35%',
      data: { team: team, isEdit: true, isOwner: true }
    });
    dialogRef.afterClosed().subscribe(_ => this.getAssignments());
  }

  private handleError(err: any): void {
    if (err.status == 409) {
      this.error = err.error
    }
    else {
      this.error = 'Error occured';
    }
  }
}
