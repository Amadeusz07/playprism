import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { Bracket } from 'src/app/models/bracket.model';

@Component({
  selector: 'app-bracket',
  templateUrl: './bracket.component.html',
  styleUrls: ['./bracket.component.scss']
})
export class BracketComponent implements OnInit, OnChanges {

  @Input() bracket: Bracket;

  constructor() { }

  ngOnInit(): void {
    
  }

  ngOnChanges() {
    console.log(this.bracket);
  }

}
