import { dateInputsHaveChanged } from "@angular/material/datepicker/datepicker-input-base";

export class UpdateTournamentRequest {
    public constructor(init?: Partial<UpdateTournamentRequest>) {
        Object.assign(this, init);
    }

    public setStartDateTimeAsUTC(input: string): void {
        const inputs = input.split(':');
        if (this.startDate) {
            this.startDate = new Date(Date.UTC(
                this.startDate.getFullYear(),
                this.startDate.getMonth(),
                this.startDate.getDate(),
                Number(inputs[0]),
                Number(inputs[1]),
                5)
            );
        }
        else {
            console.error('start date is null');
        }
    }

    public setCheckinDateTimeAsUTC(): void {
        if (this.checkInDate) {
            this.checkInDate = new Date(Date.UTC(
                this.checkInDate.getFullYear(),
                this.checkInDate.getMonth(),
                this.checkInDate.getDate(),
                10,
                0
            ));
        }
        else {
            console.error('checkInDate is null');
        }

    }

    startDate: Date | null;
    registrationEndDate: Date | string | null;
    checkInDate: Date | null;
    description: string;
    prizes: string;
    rules: string;
    contactEmail: string;
    contactNumber: string;
    registrationApprovalNeeded: boolean;
    published: boolean;
}