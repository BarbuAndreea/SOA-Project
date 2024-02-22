import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IPatient } from 'src/app/models/IPatient';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly urlMedic: string =`${environment.apiEndpoint}/medic`
  private readonly urlPatient: string =`${environment.apiEndpoint}/patient`

  constructor(private http: HttpClient) { }

  getPatient(): Observable<IPatient>{
    return this.http.get<IPatient>(this.urlPatient + "/get_patient");
  }

  getPatientsByClinic(): Observable<IPatient[]> {
    return this.http.get<IPatient[]>(this.urlMedic + "/get_all_patients");
  }

  getPatientsByName(firstName: string, lastName: string): Observable<IPatient[]>{
    let params = new HttpParams().set("firstName", firstName!).set("lastName", lastName!);
    return this.http.get<IPatient[]>(this.urlMedic + "/search_patient", {params: params});
  }

  getPatientById(id: number | undefined): Observable<IPatient> {
    return this.http.get<IPatient>(this.urlMedic + '/' + id);
  }

  getPatientByUserId(id: number | undefined): Observable<IPatient> {
    return this.http.get<IPatient>(this.urlMedic + '/get_patient_by_user/' + id)
  }

  importPatient(patientId: string): Observable<any>{
    return this.http.get<any>(`${environment.apiEndpoint}/` + 'clinicadmin/add_patient_to_clinic/'+ patientId);
  }
}
