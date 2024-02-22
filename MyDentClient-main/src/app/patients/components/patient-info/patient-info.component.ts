import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IIntervention } from 'src/app/models/IIntervention';
import { IPatient } from 'src/app/models/IPatient';
import { PatientService } from '../../service/patient.service';

@Component({
  selector: 'app-patient-info',
  templateUrl: './patient-info.component.html',
  styleUrls: ['./patient-info.component.css']
})
export class PatientInfoComponent implements OnInit {
  patientId: number | undefined;
  patient: IPatient | undefined;
  interventionList: IIntervention[] = [];
  
  constructor( private patientService: PatientService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => this.getPatientById(Number(params.get('id'))));
  }

  getPatientById(patientId: number | undefined): void {
    this.patientService.getPatientById(patientId).subscribe({
      next: (res) => {
        this.patient = res;
      },
      error: (err) => {
        console.log(err);
      }
    }
    );
  }

}
