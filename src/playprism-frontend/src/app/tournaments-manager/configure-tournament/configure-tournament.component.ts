import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatchDefinition } from 'src/app/models/match-definitions.model';
import { TournamentDetails } from 'src/app/models/tournaments/tournaments-details.model';
import { UpdateTournamentRequest } from 'src/app/models/tournaments/update-tournament-request.model';
import { MatchDefinitionService } from 'src/app/services/match-definition.service';
import { TournamentService } from 'src/app/services/tournament.service';
import { EvenNumberValidator } from 'src/app/services/validators/even-number-validator';

@Component({
  selector: 'app-configure-tournament',
  templateUrl: './configure-tournament.component.html',
  styleUrls: ['./configure-tournament.component.scss']
})
export class ConfigureTournamentComponent implements OnInit {
  public loading: boolean = true;
  public tournament: TournamentDetails;
  public matchDefinitions: MatchDefinition[];
  public get defaultMatchDefinition(): MatchDefinition {
    return this.matchDefinitions[0];
  }
  public configureFormGroup: FormGroup;
  public closeRoundResponse: string | null;
  public startTournamentError: string | null;
  
  constructor(
    private tournamentService: TournamentService, 
    private matchDefinitionService: MatchDefinitionService,
    private datepipe: DatePipe,
    private route: ActivatedRoute, 
    private formBuilder: FormBuilder) { }

  async ngOnInit(): Promise<void> {
    this.loading = true;
    const tournamentId = this.route.snapshot.paramMap.get('tournamentId');
    if (tournamentId) {
      await this.setupConfiguration(tournamentId);
    }
  }

  private async setupConfiguration(tournamentId: string): Promise<void> {
    this.tournament = await this.tournamentService.getTournament(tournamentId);
    this.matchDefinitions = await this.matchDefinitionService.getMatchDefinitions(tournamentId);
    this.configureFormGroup = this.formBuilder.group({
      startDate: this.tournament.startDate != null ? new Date(this.tournament.startDate) : null,
      startTime: this.datepipe.transform(this.tournament.startDate, 'HH:mm'),
      checkInDate: this.tournament.checkInDate,
      registrationEndDate: this.tournament.registrationEndDate,
      location: this.tournament.location,
      contactEmail: [this.tournament.contactEmail, Validators.email],
      contactNumber: [Number(this.tournament.contactNumber), Validators.pattern("^[0-9]*$")],
      description: [this.tournament.description, Validators.maxLength(256)],
      rules: [this.tournament.rules, Validators.maxLength(256)],
      prizes: [this.tournament.prizes, Validators.maxLength(256)],
      confirmationNeeded: this.defaultMatchDefinition.confirmationNeeded,
      scoreBased: [this.defaultMatchDefinition.scoreBased, Validators.required],
      bestOf: [this.defaultMatchDefinition.numberOfGames, EvenNumberValidator(false)]
    });
    this.loading = false;
  }

  public submit(): void {
    this.loading = true;
    this.saveMatchDefinition();
    const updateTournamentRequest = new UpdateTournamentRequest(this.configureFormGroup.value);
    updateTournamentRequest.setStartDateTime(this.configureFormGroup.value.startTime);
    updateTournamentRequest.published = this.tournament.published;
    this.tournamentService.putTournament(this.tournament.id.toString(), updateTournamentRequest)
      .subscribe(
        _ => this.setupConfiguration(this.tournament.id.toString()), 
        err => this.startTournamentError = err.error
      );
  }

  public saveMatchDefinition(): void {
    if (this.configureFormGroup.get('confirmationNeeded')?.touched ||
      this.configureFormGroup.get('scoreBased')?.touched || 
      this.configureFormGroup.get('bestOf')?.touched) {
        const matchDefinition = <MatchDefinition>{
          confirmationNeeded: this.configureFormGroup.get('confirmationNeeded')?.value,
          scoreBased: this.configureFormGroup.get('scoreBased')?.value,
          numberOfGames: this.configureFormGroup.get('bestOf')?.value
        }
        this.matchDefinitionService.updateMatchDefinition(
          this.tournament.id.toString(), 
          this.defaultMatchDefinition.id.toString(), 
          matchDefinition)
            .subscribe(err => console.log(err));
    }
  }

  public saveAndPublishTournament(): void {
    this.loading = true;
    this.saveMatchDefinition();
    const updateTournamentRequest = new UpdateTournamentRequest(this.configureFormGroup.value);
    updateTournamentRequest.published = true;
    this.tournamentService.putTournament(this.tournament.id.toString(), updateTournamentRequest)
      .subscribe(
        _ => this.setupConfiguration(this.tournament.id.toString()), 
        err => console.log(err)
      );
  }

  public startTournament(): void {
    if (this.tournamentValidToStart()) {
      this.tournamentService.startTournament(this.tournament.id.toString())
        .subscribe(
          _ => this.setupConfiguration(this.tournament.id.toString()),
          err => console.log(err)
      );
    }
  }

  public areDatesValid(): boolean {
    const startDate = new Date(this.configureFormGroup.get('startDate')?.value);
    const checkInDate = new Date(this.configureFormGroup.get('checkInDate')?.value);
    const registrationEndDate = new Date(this.configureFormGroup.get('registrationEndDate')?.value);
    return startDate !== null && 
      registrationEndDate != null && 
      checkInDate != null &&
      startDate > registrationEndDate && 
      startDate > checkInDate &&
      checkInDate > registrationEndDate;
  }

  public tournamentValidToStart(): boolean {
    return this.tournament.published && this.tournament.currentNumberOfPlayers >= 2 && !this.tournament.ongoing;
  }

  public closeTournament(): void {
    this.closeRoundResponse = null;
    this.tournamentService.closeCurrentRound(this.tournament.id)
      .subscribe(
        _ => {
          this.closeRoundResponse = 'Round is closed and next one is started';
          this.setupConfiguration(this.tournament.id.toString());
        },
        err => {
          if (err.status == 400) {
            this.closeRoundResponse = err.error;
          }
          else {
            this.closeRoundResponse = 'Error occured';
          }
        }
      );
  }

}
