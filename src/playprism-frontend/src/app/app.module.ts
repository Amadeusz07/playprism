import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { AuthButtonComponent } from './shared/auth-button/auth-button.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card'
import { MatInputModule } from '@angular/material/input';
import { BrowseGamesComponent } from './tournaments/browse-games/browse-games.component';
import { BrowseTournamentsComponent } from './tournaments/browse-games/browse-tournaments/browse-tournaments.component';
import { TournamentComponent } from './shared/tournament/tournament.component';
import { MatchesListComponent } from './matches/matches-list/matches-list.component';
import { MatchComponent } from './matches/matches-list/match/match.component';
import { TournamentsManagerComponent } from './tournaments-manager/tournaments-manager.component';
import { CreateTournamentComponent } from './tournaments-manager/create-tournament/create-tournament.component';
import { ConfigureTournamentComponent } from './tournaments-manager/configure-tournament/configure-tournament.component';
import { TeamsComponent } from './teams/teams.component';
import { CreateTeamComponent } from './teams/create-team/create-team.component';
import { EditTeamComponent } from './teams/edit-team/edit-team.component';
import { BracketComponent } from './shared/bracket/bracket.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthButtonComponent,
    UserProfileComponent,
    BrowseGamesComponent,
    BrowseTournamentsComponent,
    TournamentComponent,
    MatchesListComponent,
    MatchComponent,
    TournamentsManagerComponent,
    CreateTournamentComponent,
    ConfigureTournamentComponent,
    TeamsComponent,
    CreateTeamComponent,
    EditTeamComponent,
    BracketComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: 'dev-e821827o.eu.auth0.com',
      clientId: 'BfS5VVPmKwqKlWPqDqdpiisJhwtpC7sw',
      audience: 'https://playprism/api/v1',
      redirectUri: window.location.origin,
      httpInterceptor: {
        allowedList: [ 'http://localhost:5000/api/v1/*' ],
      }
    }),
    BrowserAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
