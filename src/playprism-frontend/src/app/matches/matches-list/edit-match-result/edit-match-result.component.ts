import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Match } from 'src/app/models/match.model';

@Component({
  selector: 'app-edit-match-result',
  templateUrl: './edit-match-result.component.html',
  styleUrls: ['./edit-match-result.component.scss']
})
export class EditMatchResultComponent implements OnInit {
  @Input() match: Match;
  @Output() onSubmit = new EventEmitter<Match>();
  public scoreFormGroup: FormGroup;
  public winnerScore: number;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.winnerScore = Math.floor((this.match.matchDefinition.numberOfGames / 2) + 1);
    if (this.match.matchDefinition.scoreBased) {
      this.scoreFormGroup = this.formBuilder.group({
        participant1Score: [0, [Validators.required, Validators.min(0)]],
        participant2Score: [0, [Validators.required, Validators.min(0)]],
        result: [0, [Validators.max(2), Validators.min(1)]]
      });
    }
    else {
      this.scoreFormGroup = this.formBuilder.group({
        participant1Score: [0, [Validators.required, Validators.max(this.winnerScore), Validators.min(0)]],
        participant2Score: [0, [Validators.required, Validators.max(this.winnerScore), Validators.min(0)]],
        result: [0, [Validators.max(2), Validators.min(1)]]
      });
    }
    this.scoreFormGroup.valueChanges.subscribe(_ => {
      const calculatedScore = this.calculateScore();
      this.scoreFormGroup.get('result')?.setValue(calculatedScore, { emitEvent: false });
    });
  }

  public submit(): void {
    const toEmit = {
      ...this.match,
      participant1Score: this.scoreFormGroup.value.participant1Score,
      participant2Score: this.scoreFormGroup.value.participant2Score,
      result: this.scoreFormGroup.value.result
    };
    this.onSubmit.emit(toEmit);
  }

  private calculateScore(): number {
    let result = 0;
    if (this.scoreFormGroup.value.participant1Score > this.scoreFormGroup.value.participant2Score) {
      result = 1;
    }
    else if (this.scoreFormGroup.value.participant1Score < this.scoreFormGroup.value.participant2Score) {
      result = 2;
    }
    return result;
  }

}
