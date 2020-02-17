import { FormGroup, FormControl } from "@angular/forms";
import { Component } from '@angular/core';
import { ApplicationStateService } from '../Service/application-state.service';

@Component({
    selector: 'app-base',
    template: ''
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
