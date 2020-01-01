import { Directive, Input } from "@angular/core";
import { NG_VALIDATORS, AbstractControl, Validator } from "@angular/forms";

@Directive({
    selector: '[ticketclosedate]',
    providers: [{ provide: NG_VALIDATORS, useExisting: ticketdateValidatorDirective, multi: true }]
})
export class ticketdateValidatorDirective implements Validator {
    @Input('tktclosedate') closedate: string;
    @Input('tktcreatedate') createddate: string;

    validate(control: AbstractControl): { [key: string]: any } | null {
        return null;
    }
}