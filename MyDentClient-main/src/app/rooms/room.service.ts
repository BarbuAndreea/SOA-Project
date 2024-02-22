import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IRoom } from '../models/IRoom';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private readonly url: string =`${environment.apiEndpoint}/clinicadmin`
  private readonly urlMedic: string =`${environment.apiEndpoint}/medic`
  private readonly urlPatient: string =`${environment.apiEndpoint}/patient`

  constructor(private http: HttpClient) { }

  getRooms(): Observable<IRoom[]> {
    return this.http.get<IRoom[]>(this.url + "/get-rooms");
  }

  getRoom(id: number): Observable<IRoom> {
    return this.http.get<IRoom>(this.urlPatient + "/get_room/"+ id);
  }

  getRoomsByClinic(): Observable<IRoom[]>{
    return this.http.get<IRoom[]>(this.urlMedic + "/get-rooms-by-clinic")
  }

  addRoom(room: IRoom): Observable<any> {
    return this.http.post<any>(`${this.url}/add_room`, room);
  }

  deleteRoom(id: number| undefined): Observable<IRoom> {
    return this.http.delete<IRoom>(`${this.url}/room/${id}`);
  }

  updateRoom(room: IRoom): Observable<any> {
    return this.http.put<any>(`${this.url}/edit_room`, room);
  }
}
