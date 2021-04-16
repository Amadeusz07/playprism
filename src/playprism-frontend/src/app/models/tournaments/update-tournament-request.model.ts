export class UpdateTournamentRequest {
    public constructor(init?: Partial<UpdateTournamentRequest>) {
        Object.assign(this, init);
    }

    public setStartDateTime(input: string): void {
        const inputs = input.split(':');
        if (this.startDate) {
            this.startDate.setUTCHours(Number(inputs[0]));
            this.startDate.setUTCMinutes(Number(inputs[1]));
        }
        else {
            console.error('start date is null');
        }
    }

    startDate: Date | null;
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