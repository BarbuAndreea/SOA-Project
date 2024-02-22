import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IHoliday } from '../models/IHoliday';

@Injectable({
  providedIn: 'root'
})
export class HolidayService {
  private readonly url: string = `${environment.apiEndpoint}/medic`;

  constructor(private http: HttpClient) { }

  addHoliday(holiday: IHoliday): Observable<IHoliday>{
    return this.http.post<IHoliday>(this.url + '/add-holiday', holiday)
  }

  updateHoliday(holiday: IHoliday): Observable<any>{
    return this.http.post<IHoliday>(this.url + '/update-holiday', holiday)
  }

  deleteHoliday(id: number| undefined): Observable<IHoliday> {
    return this.http.delete<IHoliday>(`${this.url}/holiday/${id}`);
  }
}
