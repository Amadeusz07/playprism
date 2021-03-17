import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { Bracket } from 'src/app/models/bracket.model';
import { CanJoin } from 'src/app/models/can-join.model';
import { TournamentFormatEnum } from 'src/app/models/enums/tournament-format.enum';
import { TournamentStatusEnum } from 'src/app/models/enums/tournament-status.enum';
import { Player } from 'src/app/models/player.model';
import { Team } from 'src/app/models/team.model';
import { TournamentDetails } from 'src/app/models/tournaments/tournaments-details.model';
import { TournamentStatusPipe } from 'src/app/pipes/tournament-status.pipe';
import { TeamService } from 'src/app/services/team.service';
import { TournamentService } from 'src/app/services/tournament.service';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html',
  styleUrls: ['./tournament.component.scss']
})
export class TournamentComponent implements OnInit {
  public tournamentFormatEnum = TournamentFormatEnum;
  public tournamentStatusEnum = TournamentStatusEnum;
  public bracket: Bracket;
  public tournament: TournamentDetails;
  public canJoin: CanJoin;
  public get tournamentStatus(): TournamentStatusEnum {
    return this.statusPipe.transform(this.tournament);
  }
  public get tournamentFormatString(): string {
    switch (this.tournament.format) {
      case TournamentFormatEnum.SingleElimination: {
        return 'Single Elimination';
      }
      case TournamentFormatEnum.League: {
        return 'League';
      }
      case TournamentFormatEnum.Groups: {
        return 'Groups';
      }
    }
  }

  private teamInfo: Team;
  private playerInfo: Player;

  constructor(
    private tournamentService: TournamentService, 
    private teamService: TeamService,
    public authService: AuthService, 
    private statusPipe: TournamentStatusPipe, 
    private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    const tournamentId = this.route.snapshot.paramMap.get('tournamentId');
    if (tournamentId) {
      await this.loadTournament(tournamentId);
    }
  }

  public async getUserTeamInfo(): Promise<void> {
    if (await this.authService.isAuthenticated$) {
      if (this.tournament.areTeams) {
        this.teamInfo = await this.teamService.getMyTeam();
      }
      else {
        this.playerInfo = await this.teamService.getPlayerInfo();
      }
    }
  }

  private async loadTournament(tournamentId: string) {
    this.bracket = await this.tournamentService.getTournamentBracket(tournamentId);
    this.tournament = await this.tournamentService.getTournament(tournamentId);
    if (await this.authService.isAuthenticated$) {
      await this.getUserTeamInfo();
      if (this.tournament.areTeams) {
        if (this.teamInfo) {
          this.canJoin = await this.tournamentService.canJoinTournament(tournamentId, this.teamInfo.id);
        }
        else {
          this.canJoin = <CanJoin>{ accepted: false, message: 'You don\'t have a team' };
        }
      }
      else {
        this.canJoin = await this.tournamentService.canJoinTournament(tournamentId, this.playerInfo.id);
      }
    }
  }

  public async join(): Promise<void> {
    if (await this.authService.isAuthenticated$ && this.canJoin.accepted) {
      if (this.tournament.areTeams) {
        await this.tournamentService.joinTournamentAsTeam(this.tournament.id, this.teamInfo);
      }
      else {
        await this.tournamentService.joinTournamentAsPlayer(this.tournament.id, this.playerInfo);
      }
      await this.loadTournament(this.tournament.id.toString());
    }
  }

}
