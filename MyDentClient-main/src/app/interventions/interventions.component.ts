import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IIntervention, ToothEnum } from '../models/IIntervention';
import { PatientService } from '../patients/service/patient.service';
import { InterventionService } from './service/intervention.service';
import { DateAdapter } from '@angular/material/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationService } from '../login/service/authentication.service';
import { UserRole } from '../models/IUser';

@Component({
  selector: 'app-interventions',
  templateUrl: './interventions.component.html',
  styleUrls: ['./interventions.component.css']
})
export class InterventionsComponent implements OnInit {
  newIntervention: IIntervention = { patientId: -1, medicId:-1, name: "", price: 0, recommendation: "", description: "", teeth: 0, date: new Date()};
  errorMessage: string = "";
  backendErrorMessage: string = "";
  teeth: Array<string> = Object.keys(ToothEnum).filter(key => isNaN(+key));
  interventionForm = this.formBuilder.group({
    date: [new Date()],
    nameInput: ['', Validators.required],
    teethInput: [null, Validators.required],
    description: [''],
    price: [0],
    recommendation: ['']
  });
  patientId!: number;
  selectedTooth!: number;
  events: string[] = [];
  isEditMode: Boolean = false;

  constructor(private interventionService: InterventionService, public dialogRef: MatDialogRef<InterventionsComponent>, private formBuilder: FormBuilder, private patientService: PatientService,
    private dateAdapter: DateAdapter<Date>,private authenticationService: AuthenticationService, @Inject(MAT_DIALOG_DATA) public data: { intervention: IIntervention, patientId: number }) { 
    this.dateAdapter.setLocale('en-GB');
  }

  ngOnInit(): void {
      this.isEditMode = this.data.intervention != null;
      if (this.isEditMode == true) {
          this.formFields['date'].setValue(this.data.intervention.date);
          this.formFields['nameInput'].setValue(this.data.intervention.name);
          this.formFields['teethInput'].setValue(this.data.intervention.teeth.toString());
          this.formFields['description'].setValue(this.data.intervention.description);
          this.formFields['price'].setValue(this.data.intervention.price);
          this.formFields['recommendation'].setValue(this.data.intervention.recommendation);
      }
  }

  onChangeDateEvent(event: any): void{
    this.newIntervention.date = event.value;
  }

  onChangeTooth(event: any): void{
    this.selectedTooth = event.target.value[0];
  }

  selectPrice(price: string): void {
    this.newIntervention.price = parseInt(price);
  }

  addIntervention(intervention: IIntervention): void {
    this.interventionService.addIntervention(intervention).subscribe({
      next: (res) => {
        this.backendErrorMessage = '';
        this.dialogRef.close('succes');
      },
      error: (err) => {
        this.backendErrorMessage = "An error ocurred and the issue was not reported! Please try again!";
      }
    });
  }

  editIntervention(intervention: IIntervention): void {
    this.interventionService.editIntervention(intervention)
      .subscribe({
        next: () => {
          this.backendErrorMessage = '';
          this.dialogRef.close('succes');
        },
        error: () => {
          this.backendErrorMessage = "An error ocurred and the issue was not reported! Please try again!";
        }
      });
  }

  get formFields() { return this.interventionForm.controls; }

  onSubmit() {
      if (this.interventionForm.invalid) {
          return;
      }

      this.newIntervention = {
        patientId: -1,
        medicId : -1,
        name: this.formFields['nameInput'].value,
        teeth: parseInt(this.formFields['teethInput'].value),
        description: this.formFields['description'].value,
        price: this.formFields['price'].value,
        recommendation: this.formFields['recommendation'].value,
        date: this.formFields['date'].value,
      }

      if (this.isEditMode == false) {
        this.newIntervention.patientId = this.data.patientId,
        this.addIntervention(this.newIntervention);
      }
      else {
        this.newIntervention.id= this.data.intervention.id,
        this.newIntervention.patientId = this.data.intervention.patientId;
        this.editIntervention(this.newIntervention);
      }

      this.interventionForm.reset();
  }
}
