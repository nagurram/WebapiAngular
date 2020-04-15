import { FormGroup, FormControl } from "@angular/forms";
import { Component } from '@angular/core';
import { ApplicationStateService } from '../Service/application-state.service';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'app-base',
    template: ''
})
export class BaseComponent {

    private _getIsMobileResolution: boolean = false;
    private _pagetitile: string = "";

    get pagetitile(): string {
        return this._pagetitile;
    }
    set pagetitile(value: string) {
        if (value !== this._pagetitile) {
            this._pagetitile = value;
            this.changetitile(value);
        }
    }

    private _pagewidth: string = "";


    get pagewidth(): string {
        return this._pagewidth;
    }
    set pagewidth(value: string) {
        if (value !== this._pagewidth) {
            this._pagewidth = value;
        }
    }

    get getIsMobileResolution(): boolean {
        return this._getIsMobileResolution;
    }
    set getIsMobileResolution(value: boolean) {
        if (value !== this._getIsMobileResolution) {
            this._getIsMobileResolution = value;
        }
    }


    constructor(protected titleService: Title) {
    }

    private changetitile(titile: string) {
        this.titleService.setTitle(titile);
    }

    validateAllFields(formGroup: FormGroup) {
       // console.log('in validations');
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

    onResize(event) {

        const innerWidth = event.target.innerWidth;
       // console.log(innerWidth);
        this.getIsMobileResolution=false;
        if (innerWidth < 992) {
          this.getIsMobileResolution=true;
        }
     }
}
