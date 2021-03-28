import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { Discipline } from 'src/app/models/discipline.model';
import { CreateTournament } from 'src/app/models/tournaments/create-tournament.model';
import { TournamentService } from 'src/app/services/tournament.service';
import { GlobalConstants } from '../../common/global-contants';

@Component({
  selector: 'app-create-tournament-dialog',
  templateUrl: './create-tournament-dialog.component.html',
  styleUrls: ['./create-tournament-dialog.component.scss']
})
export class CreateTournamentDialogComponent implements OnInit {
  public isLinear = false;
  public firstFormGroup: FormGroup;
  public secondFormGroup: FormGroup;
  public thirdFormGroup: FormGroup;
  public fourthFromGroup: FormGroup;
  public platforms = GlobalConstants.PLATFORMS
  public sizeOptions: number[] = [];
  public error: string;
  public get formIsValid(): boolean {
    return this.firstFormGroup.valid && this.secondFormGroup.valid && this.thirdFormGroup.valid && this.fourthFromGroup.valid; 
  }
  private username: string;;
  
  constructor(private tournamentService: TournamentService, 
    private authService: AuthService,
    private router: Router,
    public dialogRef: MatDialogRef<CreateTournamentDialogComponent>, 
    private formBuilder: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data: { disciplines: Discipline[] }) { }

  ngOnInit(): void {
    this.authService.user$.subscribe(userInfo => {
      this.username = userInfo.nickname;
    });
    this.createSizeOptions();
    this.firstFormGroup = this.formBuilder.group({
      tournamentName: ['', Validators.required]
    });
    this.secondFormGroup = this.formBuilder.group({
      disciplineId: ['', Validators.required],
      areTeams: false
    });
    this.thirdFormGroup = this.formBuilder.group({
      platform: ['', Validators.required]
    })
    this.fourthFromGroup = this.formBuilder.group({
      numberOfParticipants: ['', Validators.required],
      approvalNeeded: false
    })
  }

  public cancel(): void {
    this.dialogRef.close();
  }

  public submit(): void {
    this.tournamentService.postTournament(<CreateTournament> {
      name: this.firstFormGroup.value.tournamentName,
      disciplineId: this.secondFormGroup.value.disciplineId,
      areTeams: this.secondFormGroup.value.areTeams,
      platform: this.thirdFormGroup.value.platform,
      maxNumberOfPlayers: this.fourthFromGroup.value.numberOfParticipants,
      registrationApprovalNeeded: this.fourthFromGroup.value.approvalNeeded,
      ownerName: this.username
    }).subscribe(t => {
      this.router.navigate(['/manager/configure', t.id]);
      this.dialogRef.close();
    }, err => this.error = 'Something went wrong. Please check again your data');
  }

  private createSizeOptions() {
    this.sizeOptions = [GlobalConstants.MAX_TOURNAMENT_SIZE];
    while (this.sizeOptions[this.sizeOptions.length - 1] > 2) {
      const nextOption = this.sizeOptions[this.sizeOptions.length - 1] / 2;
      this.sizeOptions.push(nextOption);
    }
  }

}

