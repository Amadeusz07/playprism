import { Pipe, PipeTransform } from '@angular/core';
import { Tournament } from '../models/tournament.model';

@Pipe({
  name: 'tournamentStatus'
})
export class TournamentStatusPipe implements PipeTransform {

  transform(value: Tournament): string {
    if (value.aborted) {
      return 'Aborted';
  }
  else if (value.finished) {
      return 'Ended';
  }
  else if (value.ongoing) {
      return 'Ongoing';
  }
  else if (value.published) {
      return 'Incoming';
  }
  return 'Blocked';
  }

}
