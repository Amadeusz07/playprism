import { TeamPlayerAssignment } from "./team-player-assignment.model";

export interface Team {
    id: number;
    ownerId: string;
    name: string;
    logoPath: string;
    description: string;
    websiteUrl: string;
    contact: string;
    country: string;
    createDate: Date | string;
    deleteDate: Date | string | null;
    active: boolean;

    assignments: TeamPlayerAssignment[];
}