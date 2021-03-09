import { TournamentFormatEnum } from "./enums/tournament-format.enum";

export class Tournament {
    id: number;
    disciplineId: number;
    platform: string;
    name: string;
    ownerId: number;
    areTeams: boolean;
    maxNumberOfPlayers: number;
    timezone: string;
    startDate: string | null;
    registrationEndDate: string | null;
    checkInDate: string | null;
    checkInTime: string | null;
    location: string;
    website: string;
    logoPath: string;
    description: string;
    prizes: string;
    rules: string;
    rulesPath: string;
    contactEmail: string;
    contactNumber: string;
    format: TournamentFormatEnum;
    inviteOnly: boolean;
    registrationApprovalNeeded: boolean;
    minNumberOfPlayersInTeam: number | null;
    maxNumberOfPlayersInTeam: number | null;
    published: boolean;
    createDate: string;
    endDate: string | null;
    finished: boolean;
    aborted: boolean;
    ongoing: boolean;
}