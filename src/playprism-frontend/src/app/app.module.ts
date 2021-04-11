import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from '@auth0/auth0-angular';
import { AuthButtonComponent } from './shared/auth-button/auth-button.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card'
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs'
import { MatSelectModule } from '@angular/material/select';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatStepperModule } from '@angular/material/stepper';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSlideToggleModule } from '@angular/material/slide-toggle'
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { BrowseGamesComponent } from './tournaments/browse-games/browse-games.component';
import { BrowseTournamentsComponent } from './tournaments/browse-games/browse-tournaments/browse-tournaments.component';
import { TournamentComponent } from './shared/tournament/tournament.component';
import { MatchesListComponent } from './matches/matches-list/matches-list.component';
import { MatchComponent } from './matches/matches-list/match/match.component';
import { TournamentsManagerComponent } from './tournaments-manager/tournaments-manager.component';
import { CreateTournamentDialogComponent } from './tournaments-manager/create-tournament-dialog/create-tournament-dialog.component';
import { ConfigureTournamentComponent } from './tournaments-manager/configure-tournament/configure-tournament.component';
import { TeamsComponent } from './teams/teams.component';
import { EditTeamComponent } from './teams/edit-team/edit-team.component';
import { BracketComponent } from './shared/bracket/bracket.component';
import { AuthHttpExtendedInterceptor } from './interceptors/auth-extended.interceptor';
import { environment } from 'src/environments/environment';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TournamentStatusPipe } from './pipes/tournament-status.pipe';
import { JoinButtonComponent } from './shared/tournament/join-button/join-button.component';
import { DatePipe } from '@angular/common';
import { CreateTeamDialogComponent } from './teams/create-team-dialog/create-team-dialog.component';
import { EditMatchResultComponent } from './matches/matches-list/edit-match-result/edit-match-result.component';

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
    CreateTournamentDialogComponent,
    ConfigureTournamentComponent,
    TeamsComponent,
    EditTeamComponent,
    BracketComponent,
    TournamentStatusPipe,
    JoinButtonComponent,
    CreateTeamDialogComponent,
    EditMatchResultComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AuthModule.forRoot({
      domain: 'dev-e821827o.eu.auth0.com',
      clientId: 'BfS5VVPmKwqKlWPqDqdpiisJhwtpC7sw',
      audience: 'https://playprism/api/v1',
      redirectUri: window.location.origin,
      httpInterceptor: {
        allowedList: [ 
          `${environment.API_URL_TEAM}/*`, 
          `${environment.API_URL_TOURNAMENT}/*` 
        ],
      }
    }),
    BrowserAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatTabsModule,
    MatOptionModule,
    MatNativeDateModule,
    MatSelectModule,
    MatListModule,
    MatTableModule,
    MatStepperModule,
    MatDialogModule,
    MatSlideToggleModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatExpansionModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpExtendedInterceptor, multi: true },
    TournamentStatusPipe,
    DatePipe
  ],
  bootstrap: [AppComponent],
  exports: [
    TournamentStatusPipe
  ]
})
export class AppModule { }
