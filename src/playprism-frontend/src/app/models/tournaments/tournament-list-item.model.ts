import { TournamentFormatEnum } from "../enums/tournament-format.enum";
import { Tournament } from "./tournament.model";

export interface TournamentListItem extends Tournament{
    id: number;
    disciplineName: string;
    name: string;
    ownerName: string;
    startDate: Date | string;
    currentNumberOfPlayers: number;
    maxNumberOfPlayers: number;
    ongoing: boolean;
    finished: boolean;
    aborted: boolean;
    published: boolean;
    format: TournamentFormatEnum;
}