export interface Bracket {
    rounds: Round[];
}

export interface Round {
    id: number;
    matches: Match[];
    roundDate: Date | null;
}

export interface Match {
    id: number;
    participant1: string;
    participant2: string;
    participant1Score: number | null;
    participant2Score: number | null;
    result: number | null;
    matchDate: Date | null;
}