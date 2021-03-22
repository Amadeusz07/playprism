export interface CreateTournament {
    name: string;
    disciplineId: number;
    areTeams: boolean;
    platform: string;
    maxNumberOfPlayers: number;
    registrationApprovalNeeded: boolean;
    ownerName: string;
}