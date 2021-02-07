import { Component, Input, OnInit } from '@angular/core';
import { Round } from 'src/app/models/bracket.model';

@Component({
  selector: 'app-bracket',
  templateUrl: './bracket.component.html',
  styleUrls: ['./bracket.component.scss']
})
export class BracketComponent implements OnInit {
  @Input() bracket: Round[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
