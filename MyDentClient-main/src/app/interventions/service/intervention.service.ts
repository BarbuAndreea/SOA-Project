import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IIntervention } from 'src/app/models/IIntervention';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InterventionService{
  private readonly url: string = `${environment.apiEndpoint}/medic`

  constructor(private http: HttpClient) { }

  addIntervention(intervention: IIntervention): Observable<any> {
    return this.http.post<any>(this.url + '/addIntervention', intervention);
  }

  editIntervention(intervention: IIntervention): Observable<any>{
    return this.http.post<IIntervention>(this.url + '/updateIntervention', intervention)
  }

  getInterventionsByPatientId(id: number): Observable<IIntervention[]> {
    return this.http.get<IIntervention[]>(this.url + '/pacientIntervention/' + id);
  }

  getInterventionsByPatient(): Observable<IIntervention[]>{
    return this.http.get<IIntervention[]>(`${environment.apiEndpoint}/patient/patinet_intervetions`);
  }
}
