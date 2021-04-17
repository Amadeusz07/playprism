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
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { team: Team, isEdit: boolean },
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

  public deleteTeam(team: Team): void {
    this.teamService.deleteTeam(team).subscribe(
      _ => {
        this.dialogRef.close();
      },
      err => this.error = err.error
    );
  }
}
