import { Player } from "./player.model";
import { Team } from "./team.model";

export interface TeamPlayerAssignment {
    playerId: number;
    teamId: number;
    inviteDate: Date | string;
    responseDate: Date | string | null;
    leaveDate: Date | string | null;
    accepted: boolean;
    active: boolean;
    isOwner: boolean;
    player: Player;
    team: Team;
}