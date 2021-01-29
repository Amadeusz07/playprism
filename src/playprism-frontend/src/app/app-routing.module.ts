import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { BrowseGamesComponent } from './tournaments/browse-games/browse-games.component';
import { BrowseTournamentsComponent } from './tournaments/browse-games/browse-tournaments/browse-tournaments.component';
import { TournamentComponent } from './shared/tournament/tournament.component';
import { MatchesListComponent } from './matches/matches-list/matches-list.component';
import { MatchComponent } from './matches/matches-list/match/match.component';
import { TournamentsManagerComponent } from './tournaments-manager/tournaments-manager.component';
import { CreateTournamentComponent } from './tournaments-manager/create-tournament/create-tournament.component';
import { ConfigureTournamentComponent } from './tournaments-manager/configure-tournament/configure-tournament.component';
import { CreateTeamComponent } from './teams/create-team/create-team.component';
import { EditTeamComponent } from './teams/edit-team/edit-team.component';
import { TeamsComponent } from './teams/teams.component';

const routes: Routes = [
  {
    path: 'browse/games',
    children: [
      { path: '', component: BrowseGamesComponent },
      {
        path: ':gameId/tournaments',
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
      { path: 'create-tournament', component: CreateTournamentComponent },
      { path: 'configure/:id', component: ConfigureTournamentComponent }
    ]
  },
  {
    path: 'teams',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: TeamsComponent },
      { path: 'create-team', component: CreateTeamComponent },
      { path: 'edit/:id', component: EditTeamComponent }
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
