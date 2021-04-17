import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Team } from 'src/app/models/team.model';

@Component({
  selector: 'app-team-form',
  templateUrl: './team-form.component.html',
  styleUrls: ['./team-form.component.scss']
})
export class TeamFormComponent implements OnInit {
  @Input() team: Team | null;
  @Input() deleteButtonVisible: boolean;
  @Output() onSubmit = new EventEmitter<Team>();
  @Output() onDelete = new EventEmitter<Team>();

  public teamFormGroup: FormGroup;
  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    console.log(this.team);
    this.teamFormGroup = this.formBuilder.group({
      teamName: [this.team?.name, Validators.required],
      description: [this.team?.description],
      contact: [this.team?.contact]
    });
  }

  public submit(): void {
    const toEmit = <Team>{
      ...this.team,
      name: this.teamFormGroup.value.teamName,
      description: this.teamFormGroup.value.description,
      contact: this.teamFormGroup.value.contact
    };
    this.onSubmit.emit(toEmit);
  }

  public deleteTeam(): void {
    this.onDelete.emit(<Team>{ ...this.team });
  }

}
