import { TeamPlayerAssignment } from "./team-player-assignment.model";

export interface Player {
    id: number;
    userId: string;
    name: string;

    assignments: TeamPlayerAssignment[];
}