import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { Discipline } from 'src/app/models/discipline.model';
import { Player } from 'src/app/models/player.model';
import { CreateTournament } from 'src/app/models/tournaments/create-tournament.model';
import { TeamService } from 'src/app/services/team.service';
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
  private playerInfo: Player;
  public get formIsValid(): boolean {
    return this.firstFormGroup.valid && this.secondFormGroup.valid && this.thirdFormGroup.valid && this.fourthFromGroup.valid; 
  }

  constructor(private tournamentService: TournamentService, 
    private authService: AuthService,
    private teamService: TeamService,
    private router: Router,
    public dialogRef: MatDialogRef<CreateTournamentDialogComponent>, 
    private formBuilder: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data: { disciplines: Discipline[] }) { }

  ngOnInit(): void {
    this.getUserTeamInfo()
    this.createSizeOptions();
    this.firstFormGroup = this.formBuilder.group({
      tournamentName: ['', Validators.required]
    });
    this.secondFormGroup = this.formBuilder.group({
      discipline: ['', Validators.required],
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
      disciplineId: this.secondFormGroup.value.discipline.id,
      areTeams: this.secondFormGroup.value.areTeams,
      platform: this.thirdFormGroup.value.platform,
      maxNumberOfPlayers: this.fourthFromGroup.value.numberOfParticipants,
      registrationApprovalNeeded: this.fourthFromGroup.value.approvalNeeded,
      ownerName: this.playerInfo.name
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

  public async getUserTeamInfo(): Promise<void> {
    if (await this.authService.isAuthenticated$) {
        this.playerInfo = await this.teamService.getPlayerInfo();
    }
  }

}

