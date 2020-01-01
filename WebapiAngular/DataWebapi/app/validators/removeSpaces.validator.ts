import { AbstractControl } from '@angular/forms';

export function removeSpaces(control: AbstractControl):any {
    if (control && control.value && !control.value.replace(/\s/g, '').length) {
        console.log('in remove spaces function');
        console.log(control.value);
        control.setValue('');
    }
    return null;
}