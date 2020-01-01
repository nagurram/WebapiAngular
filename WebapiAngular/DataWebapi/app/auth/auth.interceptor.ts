import { HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "../Service/user.service";
import 'rxjs/add/operator/do';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Location } from '@angular/common';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router, private location: Location) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.headers.get('No-Auth') == "True")
            return next.handle(req.clone());

        if (localStorage.getItem('userToken') != null) {
            const clonedreq = req.clone({
                headers: req.headers.set("Authorization", "Bearer " + localStorage.getItem('userToken'))
            });
            return next.handle(clonedreq)
                .do(
                    succ => { },
                    err => {
                        if (err.status === 401)
                            if (localStorage.getItem('userToken') != null) {
                               // this.location.back();
                                this.router.navigateByUrl('/NotAuthorized');
                            }
                            else {
                                this.router.navigateByUrl('/login');
                            }
                    }
                );
        }
        else {
            this.router.navigateByUrl('/login');
        }
    }
}