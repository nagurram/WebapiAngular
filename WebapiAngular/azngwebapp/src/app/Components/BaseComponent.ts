import { FormGroup, FormControl } from "@angular/forms";
import { Component } from '@angular/core';


@Component({
    selector: 'app-base',
    template: `
        <div>
            base works!!
        </div>
    `
})
export class BaseComponent {
    validateAllFields(formGroup: FormGroup) {
        Object.keys(formGroup.controls).forEach(field => {
            const control = formGroup.get(field);
            if (control instanceof FormControl) {
                control.markAsTouched({ onlySelf: true });
            }
            else if (control instanceof FormGroup) {
                this.validateAllFields(control);
            }
        });
    }
}
