import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/team.model';
import { TeamService } from 'src/app/services/team.service';
import { CreateTournamentDialogComponent } from 'src/app/tournaments-manager/create-tournament-dialog/create-tournament-dialog.component';

@Component({
  selector: 'app-create-team-dialog',
  templateUrl: './create-team-dialog.component.html',
  styleUrls: ['./create-team-dialog.component.scss']
})
export class CreateTeamDialogComponent implements OnInit {
  public createTeamFormGroup: FormGroup;
  public error: string;
  constructor(
    private teamService: TeamService, 
    public dialogRef: MatDialogRef<CreateTournamentDialogComponent>, 
    private formBuilder: FormBuilder, 
    private router: Router) { }

  ngOnInit(): void {
    this.createTeamFormGroup = this.formBuilder.group({
      teamName: ['', Validators.required],
      description: [''],
      contact: ['']
    });
  }

  public cancel(): void {
    this.dialogRef.close();
  }

  public submit(): void {
    const team = <Team>{
      name: this.createTeamFormGroup.value.teamName,
      description: this.createTeamFormGroup.value.description,
      contact: this.createTeamFormGroup.value.contact
    }
    this.teamService.postTeam(team).subscribe(
      t => {
        this.router.navigate(['/teams/edit', t.id]);
        this.dialogRef.close();
      },
      // err => this.error = 'Something went wrong. Please check again your data'
      err => this.error = err.error
    )
  }
}
