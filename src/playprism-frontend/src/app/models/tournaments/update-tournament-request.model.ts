export class UpdateTournamentRequest {
    public constructor(init?: Partial<UpdateTournamentRequest>) {
        Object.assign(this, init);
    }

    startDate: Date | string | null;
    registrationEndDate: Date | string | null;
    checkInDate: Date | string | null;
    description: string;
    prizes: string;
    rules: string;
    contactEmail: string;
    contactNumber: string;
    registrationApprovalNeeded: boolean;
    published: boolean;
}