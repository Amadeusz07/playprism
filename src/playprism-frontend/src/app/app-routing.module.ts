import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { BrowseGamesComponent } from './tournaments/browse-games/browse-games.component';
import { BrowseTournamentsComponent } from './tournaments/browse-games/browse-tournaments/browse-tournaments.component';
import { TournamentComponent } from './shared/tournament/tournament.component';
import { MatchesListComponent } from './matches/matches-list/matches-list.component';
import { MatchComponent } from './matches/matches-list/match/match.component';
import { TournamentsManagerComponent } from './tournaments-manager/tournaments-manager.component';
import { ConfigureTournamentComponent } from './tournaments-manager/configure-tournament/configure-tournament.component';
import { TeamsComponent } from './teams/teams.component';
import { PlayerStatisticsComponent } from './player-statistics/player-statistics.component';

const routes: Routes = [
  {
    path: 'browse/games',
    children: [
      { path: '', component: BrowseGamesComponent },
      {
        path: ':disciplineId/tournaments',
        children: [
          {  path: '', component: BrowseTournamentsComponent },
          {  path: ':tournamentId', component: TournamentComponent }
        ]
      }
    ]
  },
  {
    path: 'matches',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: MatchesListComponent },
      { path: ':id', component: MatchComponent }
    ]
  },
  {
    path: 'manager',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: TournamentsManagerComponent },
      { path: 'configure/:tournamentId', component: ConfigureTournamentComponent }
    ]
  },
  {
    path: 'teams',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: TeamsComponent }
    ]
  },
  {
    path: 'player-statistics',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: PlayerStatisticsComponent }
    ]
  },
  { path: '',   redirectTo: '/browse/games', pathMatch: 'full' },
  { path: '**', redirectTo: '/browse/games' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
