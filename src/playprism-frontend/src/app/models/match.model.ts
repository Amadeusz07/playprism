import { MatchDefinition } from "./match-definitions.model";

export interface Match {
    id: number;
    matchDefinition: MatchDefinition; 
    matchDate: Date;
    participant1Name: string; 
    participant2Name: string;
    participant1Id: number;
    participant2Id: number;
    participant1Score: number;
    participant2Score: number;
    result: number;
}