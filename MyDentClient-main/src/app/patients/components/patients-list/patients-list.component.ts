import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { IPatient } from 'src/app/models/IPatient';
import { PatientService } from '../../service/patient.service';

@Component({
  selector: 'app-patients-list',
  templateUrl: './patients-list.component.html',
  styleUrls: ['./patients-list.component.css']
})
export class PatientsListComponent implements OnInit {
  patientList: IPatient[] = [];
  selectedPatientId!: number;
  errorMessage: string = '';
  searchPatientForm = this.formBuilder.group({
    firstNameInput: ['', Validators.required],
    lastNameInput: ['', Validators.required]
  });

  constructor(private patientService: PatientService, private formBuilder: FormBuilder){ }

  ngOnInit(): void {
    this.getPatients();
  }

  get formFields() { return this.searchPatientForm.controls; }

  getPatients(): void {
    this.errorMessage = "";
    this.patientService.getPatientsByClinic().subscribe({
      next: (res) => {
        this.patientList = res;
        this.searchPatientForm.reset();
      },
      error: (err) => {
        console.log(err);
      }
    }
    )
  }

  getPatientsByName(firstName: string, lastName: string): void {
    this.patientService.getPatientsByName(firstName, lastName).subscribe({
      next: (res) => {
        this.errorMessage = "";
        this.patientList = res;
      },
      error: (err) => {
        this.errorMessage = err;
        console.log(err);
      }
    }
    )
  }

  switchSelectedPatient(patientId: number): void {
    this.selectedPatientId = patientId;
  }

  onValueSelected(patientId: number): void {
    this.switchSelectedPatient(patientId);
  }

  onSubmit(): void {
    this.getPatientsByName(this.searchPatientForm.value.firstNameInput, this.searchPatientForm.value.lastNameInput);
    if (this.patientList.length == 0)
    {
      this.errorMessage = "No patient with the given name!";
      this.getPatients();
    }
  }
}
