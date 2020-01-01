import { Component, transition, OnInit } from '@angular/core';
import { Itdictionary } from './Model/tdictionary';
import { Title } from '@angular/platform-browser';
import { AuthGuard } from './auth/auth.guard';
import { UserService } from './Service/user.service';
import { IkeyValuePair } from './Model/keyValuePair';
import { MenuComponent } from './Components/menu.component';

@Component({
    selector: 'my-app',
    template: `
<menu-items></menu-items>
                    <div class='container'>
                        <router-outlet></router-outlet>
                    </div>
                `
})
export class AppComponent implements OnInit {
    msg: string;

    constructor(private titleService: Title) {
    }

    public setTitle(newTitle: string) {
        this.titleService.setTitle(newTitle);
    }

    ngOnInit() {
    }
    private isloggedin(): boolean {
        if (localStorage.getItem('userToken') != null) {
            return true;
        }
        else {
            return false;
        }
    }
}
