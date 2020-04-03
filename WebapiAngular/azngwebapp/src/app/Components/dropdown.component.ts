import { Component, Input, SimpleChange, OnInit, forwardRef } from '@angular/core';
import { IkeyValuePair } from '../Model/keyValuePair';
import { Form, FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'dropdown',
    template: ` <select  class="form-control" [disabled]="disabled"   (change)="onChange($event.target.value)" (blur)="onTouched()" >
                                <option value=-1>Select</option>
                                <option *ngFor="let coll of drpcollection" [value]="coll.Id" [selected]="coll.Id ==value" >{{coll.keyValue}}</option>
                            </select>`,
    providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => DropdownComponent), multi: true }]
})

export class DropdownComponent implements ControlValueAccessor {

    value: number;
    onChange: () => void;
    onTouched: () => void;
    disabled: boolean;
    show: boolean = false;
    constructor() { }

    @Input() drpcollection: IkeyValuePair[];

    writeValue(value: any): void {
        //console.log('in write vaule:'+value);
        this.value = value || -1;
    }

    registerOnChange(fn: any): void { this.onChange = fn; }
    registerOnTouched(fn: any): void { this.onTouched = fn; }
    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }
}