import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnChanges } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnChanges {

  public profile: string = "";
  private tmp: any;

  constructor(public auth: AuthService, private http: HttpClient) {}

  public ngOnChanges() {
    this.auth.user$.subscribe(ele => {
      console.log(ele);
      this.tmp = ele;
    });
  }

  public runRequest(): void {
    this.http.get<any>("http://localhost:5000/api/v1/WeatherForecast", {
    }).subscribe(result => {
      this.profile = result;
    });
  }

}
