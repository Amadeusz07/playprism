import { AbstractControl, ValidatorFn } from "@angular/forms";

export function EvenNumberValidator(shouldBeEven: boolean): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
        const isEven = (control.value % 2 == 0);
        if (isEven == shouldBeEven) {
            return null;
        }
        else {
            return { 'EvenNumberValidator': true };
        }
    };
}