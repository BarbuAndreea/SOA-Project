import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IClinic } from 'src/app/models/IClinic';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {
  private readonly url: string =`${environment.apiEndpoint}/superadmin`
  private readonly urlPatient: string =`${environment.apiEndpoint}/patient`

  constructor(private http: HttpClient) { }

  getClinics(): Observable<IClinic[]> {
    return this.http.get<IClinic[]>(this.url + "/get-clinics");
  }

  getClinicsByPatient(): Observable<IClinic[]> {
    return this.http.get<IClinic[]>(this.urlPatient + "/get_clinics_by_patient");
  }

  addClinic(clinic: IClinic): Observable<any> {
    return this.http.post<any>(`${this.url}/add_clinic`, clinic);
  }

  deleteClinic(id: number| undefined): Observable<IClinic> {
    return this.http.delete<IClinic>(`${this.url}/clinic/${id}`);
  }

  updateClinic(clinic: IClinic): Observable<any> {
    return this.http.put<any>(`${this.url}/update_clinic`, clinic);
  }

}
