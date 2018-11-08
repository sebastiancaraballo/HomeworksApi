import { Injectable } from '@angular/core';
import { Headers } from "@angular/http";

@Injectable({
  providedIn: 'root'
})
export class SessionService {
    constructor() { }

    logIn(token:string):void {
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('token', token);
    }

    logOut():void {
       localStorage.clear();
       localStorage.setItem('isLoggedIn', 'false');
    }

    isLoggedIn():boolean {
        return localStorage.getItem('isLoggedIn') == 'true';
    }

    getUser(): String {
        return 'Santiago';
    }

    addTokenToHeaders(headers:Headers):Headers {
        if (localStorage.getItem('token')) {
            headers.append('Authorization', localStorage.getItem('token'));
        }
        return headers;
    }
}

