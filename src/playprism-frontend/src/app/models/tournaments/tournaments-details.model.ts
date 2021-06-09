import { TournamentFormatEnum } from "../enums/tournament-format.enum";
import { Tournament } from "./tournament.model";

export interface TournamentDetails extends Tournament {
    id: number;
    platform: string;
    name: string;
    disciplineName: string;
    ownerName: string;
    areTeams: boolean;
    currentNumberOfPlayers: number;
    maxNumberOfPlayers: number;
    timezone: string;
    startDate: string | Date | null;
    registrationEndDate: Date | string | null;
    checkInDate: Date | string | null;
    checkInTime: Date | string | null;
    location: string;
    website: string;
    logoPath: string;
    description: string;
    prizes: string;
    rules: string;
    contactEmail: string;
    contactNumber: string;
    format: TournamentFormatEnum;
    inviteOnly: boolean;
    registrationApprovalNeeded: boolean;
    published: boolean;
    endDate: Date | string | null;
    ongoing: boolean;
    finished: boolean;
    aborted: boolean;
}