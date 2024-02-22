import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { IMedic, Specializations } from 'src/app/models/IMedic';
import { IUser } from 'src/app/models/IUser';
import { UserService } from 'src/app/users/service/user.service';
import { MedicService } from '../medic.service';

@Component({
  selector: 'app-edit-info-medic',
  templateUrl: './edit-info-medic.component.html',
  styleUrls: ['./edit-info-medic.component.css']
})
export class EditInfoMedicComponent implements OnInit {
  newMedic!: IMedic;
  newUser!: IUser;
  backendErrorMessage: string = '';
  dateNow: Date = new Date();
  startWorkingHour: Date = new Date();
  endWorkingHour: Date = new Date();
  specializations: Array<string> = Object.keys(Specializations).filter(key => isNaN(+key));
  editMedicInfoForm = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    age: [''],
    phone: ['', Validators.required],
    email: ['', [Validators.email, Validators.required]],
  });
  submitted = false;

  constructor(private medicService: MedicService, private authenticationService: AuthenticationService, private userService: UserService, public dialogRef: MatDialogRef<EditInfoMedicComponent>, private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA) public data: { medic: IMedic }) { }

  get formFields() { return this.editMedicInfoForm.controls; }

  ngOnInit(): void {
    this.formFields['firstName'].setValue(this.data.medic.userM.firstName);
    this.formFields['lastName'].setValue(this.data.medic.userM.lastName);
    this.formFields['age'].setValue(this.data.medic.userM.age);
    this.formFields['phone'].setValue(this.data.medic.userM.phoneNumber);
    this.formFields['email'].setValue(this.data.medic.userM.email);
    this.startWorkingHour = new Date(this.data.medic.startWorkingHour);
    this.endWorkingHour = new Date(this.data.medic.endWorkingHour);
  }

  onSubmit() {
    this.submitted = true;
    this.newUser = {
      id: this.data.medic.userM.id,
      firstName: this.formFields['firstName'].value,
      lastName: this.formFields['lastName'].value,
      age: this.formFields['age'].value,
      email: this.formFields['email'].value,
      phoneNumber: this.formFields['phone'].value,
      password: this.data.medic.userM.password,
      role: this.data.medic.userM.role,
      token: this.authenticationService.currentUserValue.token
    }
    this.startWorkingHour.setFullYear(this.dateNow.getFullYear());
    this.endWorkingHour.setFullYear(this.dateNow.getFullYear());
    this.startWorkingHour.setSeconds(0);
    this.endWorkingHour.setSeconds(0);
    this.newMedic = {
      id: this.data.medic.id,
      userM: this.newUser,
      startWorkingHour: new Date(this.startWorkingHour),
      endWorkingHour: new Date(this.endWorkingHour),
      clinicId: this.data.medic.clinicId,
      specialization: this.data.medic.specialization,
      holidays: this.data.medic.holidays
    };

    this.medicService.updateMedic(this.newMedic)
      .subscribe({
        next: () => {
          this.backendErrorMessage = '';
          this.dialogRef.close();
        },
        error: (err) => {
          this.backendErrorMessage = err;
        }
      });
  }

}