export interface MatchDefinition {
    id: number;
    tournamentId: number;
    name: string;
    confirmationNeeded: boolean;
    numberOfGames: number;
    scoreBased: boolean;
}