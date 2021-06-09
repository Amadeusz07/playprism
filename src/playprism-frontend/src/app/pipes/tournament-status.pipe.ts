import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { TournamentStatusEnum } from '../models/enums/tournament-status.enum';
import { Tournament } from '../models/tournaments/tournament.model';

@Pipe({
  name: 'tournamentStatus'
})
@Injectable({
  providedIn: 'root'
})
export class TournamentStatusPipe implements PipeTransform {

  transform(value: Tournament): TournamentStatusEnum {
    if (value.aborted) {
      return TournamentStatusEnum.Aborted;
    }
    else if (value.finished) {
        return TournamentStatusEnum.Ended;
    }
    else if (value.ongoing) {
        return TournamentStatusEnum.Ongoing;
    }
    else if (value.published) {
        return TournamentStatusEnum.Incoming;
    }
    return TournamentStatusEnum.Blocked;
  }

}
