import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Team } from 'src/app/models/team.model';
import { TeamService } from 'src/app/services/team.service';

@Component({
  selector: 'app-team-dialog',
  templateUrl: './team-dialog.component.html',
  styleUrls: ['./team-dialog.component.scss']
})
export class TeamDialogComponent implements OnInit {
  public error: string;
  public team: Team;
  public usernameToInvite: string;
  public inviteMessage: string;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { team: Team, isEdit: boolean, isOwner: boolean },
    private teamService: TeamService, 
    public dialogRef: MatDialogRef<TeamDialogComponent>) { }

  ngOnInit(): void {
  }

  public cancel(): void {
    this.dialogRef.close();
  }

  public submit(team: Team): void {
    if (this.data.isEdit) {
      this.teamService.putTeam(team).subscribe(
        t => {
          this.dialogRef.close();
        },
        err => this.error = err.error
      )
    }
    else {
      this.teamService.postTeam(team).subscribe(
        t => {
          this.dialogRef.close();
        },
        err => this.error = err.error
      )
    }
  }

  public async invite(teamId: number): Promise<void> {
    this.inviteMessage = '';
    this.error = '';
    this.teamService.sendInvite(teamId, this.usernameToInvite).subscribe(
      _ => this.inviteMessage = 'User has been invited',
      err => {
        if (err.status == 409) {
          this.error = err.error;
        }
        else {
          this.error = 'Can\'t invite user'
        }
      }
    );
  }

  public deleteTeam(team: Team): void {
    this.teamService.deleteTeam(team).subscribe(
      _ => {
        this.dialogRef.close();
      },
      err => this.error = err.error
    );
  }
}
