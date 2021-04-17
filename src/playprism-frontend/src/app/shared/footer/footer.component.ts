import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  public year: number;

  constructor(public auth: AuthService) { }

  ngOnInit(): void {
    this.year = new Date().getFullYear();
  }

}
