import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { IUser } from 'src/app/models/IUser';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<IUser>;
    public currentUser: Observable<IUser>;
    private jwtHelper = new JwtHelperService();

    constructor(private http: HttpClient, private router: Router) {
        this.currentUserSubject = new BehaviorSubject<IUser>(JSON.parse(localStorage.getItem('currentUser')||'{}'));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): IUser {
        return this.currentUserSubject.value;
    }

    public isAuthenticated(): boolean {
      const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
      return !this.jwtHelper.isTokenExpired(currentUser.token);
    }

    login(email: string, password: string) {
        return this.http.post<any>(`${environment.apiEndpoint}/user/authenticate`, { email, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        this.currentUserSubject.value.token = '';
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
    }
}