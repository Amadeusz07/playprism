export interface Round {
    matches: Match[];
}

export interface Match {
    participant1: string;
    participant2: string;
    participantScore1: number | null;
    participantScore2: number | null;
}