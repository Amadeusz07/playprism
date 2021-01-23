import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { AuthButtonComponent } from './shared/auth-button/auth-button.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    AuthButtonComponent,
    UserProfileComponent
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
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
