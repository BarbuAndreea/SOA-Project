import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/models/IUser';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  private readonly url: string =`${environment.apiEndpoint}/user`

  constructor(private http: HttpClient) { }

  addUser(user: IUser): Observable<any> {
    return this.http.post<any>(this.url + "/newUser", user);
  }
}
