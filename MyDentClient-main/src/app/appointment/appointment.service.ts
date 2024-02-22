import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AppointmentStatus, IAppointment } from '../models/IAppointment';
import { UserRole } from '../models/IUser';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly url: string = `${environment.apiEndpoint}/medic`

  constructor(private http: HttpClient) { }

  displayAppointmentsByDate(date: Date): Observable<IAppointment[]> {
    return this.http.post<IAppointment[]>(this.url + '/get-appointments-by-date', date);
  }

  addAppointment(appointment: IAppointment): Observable<any> {
    return this.http.post<IAppointment>(this.url + '/addAppointment', appointment)
  }

  editAppointment(appointment: IAppointment): Observable<IAppointment> {
    return this.http.post<IAppointment>(this.url + '/updateAppointment', appointment)
  }

  editAppointmentStatus(appointment: IAppointment): Observable<any> {
    return this.http.post<IAppointment>(this.url + '/updateAppointmentStatus', appointment)
  }

  getNextAppointmentsByPatient(): Observable<IAppointment[]> {
    return this.http.get<IAppointment[]>(`${environment.apiEndpoint}/patient/get_next_appointments_by_patient`);
  }

  getPastAppointmentsByPatient(): Observable<IAppointment[]> {
    return this.http.get<IAppointment[]>(`${environment.apiEndpoint}/patient/get_past_appointments_by_patient`);
  }

  getAppointmentsByStatus(status: AppointmentStatus): Observable<IAppointment[]> {
    return this.http.get<IAppointment[]>(this.url + '/appointments_by_status/' + status);
  }

  deleteAppointmentById(id: number | undefined): Observable<any> {
    return this.http.delete<IAppointment>(`${this.url}/appointment/${id}`);
  }
}
