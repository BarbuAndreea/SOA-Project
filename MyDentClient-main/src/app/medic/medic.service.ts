import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IMedic } from '../models/IMedic';

@Injectable({
  providedIn: 'root'
})
export class MedicService {
  private readonly urlP: string = `${environment.apiEndpoint}/patient`
  private readonly urlM: string = `${environment.apiEndpoint}/medic`
  private readonly urlC: string = `${environment.apiEndpoint}/clinicadmin`

  constructor(private http: HttpClient) { }

  getMedicById(medicId: number): Observable<IMedic>{
    return this.http.get<IMedic>(this.urlP + '/get_medic_by_id/' + medicId)
  }

  getMedicByUserId(): Observable<IMedic>{
    return this.http.get<IMedic>(this.urlM + '/get_medic')
  }

  updateMedic(medic: IMedic): Observable<any>{
    return this.http.post<IMedic>(this.urlM + '/update_medic_info', medic);
  }

  getMedicsByClinic(): Observable<IMedic[]>{
    return this.http.get<IMedic[]>(this.urlC + '/get_medics_by_clinic')
  }
}
