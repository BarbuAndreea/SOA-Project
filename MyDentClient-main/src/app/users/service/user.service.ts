import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IUser, IUserToAdd } from 'src/app/models/IUser';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly url: string = `${environment.apiEndpoint}/user`
    private readonly urlSuperAdmin: string = `${environment.apiEndpoint}/superadmin`
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<IUser[]>(this.url);
    }

    addUser(user: IUserToAdd): Observable<any> {
        return this.http.post<any>(this.url + "/newUser", user);
    }

    changePassword(password: string): Observable<any> {
        return this.http.put<any>(`${this.url}/change_password`, { password });
    }
    getUserByName(firstName: string, lastName: string): Observable<IUser[]> {
        let params = new HttpParams().set("firstName", firstName!).set("lastName", lastName!);
        return this.http.get<IUser[]>(this.urlSuperAdmin + "/search_users", { params: params });
    }
}