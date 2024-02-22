import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { IClinic } from 'src/app/models/IClinic';
import { Specializations } from 'src/app/models/IMedic';
import { IUserToAdd, UserRole } from 'src/app/models/IUser';
import { UserService } from '../../service/user.service';
import { ClinicService } from 'src/app/clinic/service/clinic.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  addUserForm!: FormGroup;
  submitted = false;
  errorMessage = '';
  showSpecial = false;
  showClinicSelect = false;
  user!: IUserToAdd;
  userRoles: Array<string> = Object.keys(UserRole).filter(key => isNaN(+key));
  specializations: Array<string> = Object.keys(Specializations).filter(key => isNaN(+key));
  clinics: IClinic[] = [];
  selectedRole!: number;
  selectedSpecial: number | undefined;
  selectedClinic: number | undefined;
  currentUserRole: UserRole | undefined;

  constructor(private userService: UserService, private snackBar: MatSnackBar, private clinicService: ClinicService, private formBuilder: FormBuilder, private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.currentUserRole = this.authenticationService.currentUserValue.role;
    this.addUserForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      age: [''],
      phone: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      password: ['', Validators.required],
      role: [null, Validators.required],
      specialization: [null],
      clinic: [null]
    });
  }

  onChangeRole(event: any) {
    this.selectedRole = event.target.value[0];
    if (this.selectedRole == 1) {
      this.showSpecial = true;
      this.showClinicSelect = true;
      this.clinicService.getClinics().subscribe({
        next: (res) => {
          this.clinics = res;
          this.errorMessage = '';
        },
        error: (err) => {
        }
      })
    }
    else if (this.selectedRole == 0 || this.selectedRole == 2) {
      this.showSpecial = false;
      this.showClinicSelect = false;
    } else if (this.selectedRole == 3) {
      this.clinicService.getClinics().subscribe({
        next: (res) => {
          this.clinics = res;
          this.errorMessage = '';
        },
        error: (err) => {
        }
      })
      this.showClinicSelect = true;
    }
  }

  onChangeSpecialization(event: any) {
    this.selectedSpecial = event.target.value;
  }

  onChangeClinic(event: any) {
    this.selectedClinic = event.target.value;
  }

  public get UserRole() {
    return UserRole;
  }

  get formFields() { return this.addUserForm.controls; }

  onSubmit(formDirective: FormGroupDirective) {
    this.submitted = true;

    if (this.addUserForm.invalid) {
      return;
    }

    var clinicField = this.formFields['clinic'].value
    var clinicId = clinicField == null ? -1 : clinicField
    var specializationField = this.formFields['specialization'].value;
    var specialization = specializationField == null ? 0 : (<any>Specializations)[specializationField];
    var roleField = this.formFields['role'].value;
    var role = (<any>UserRole)[roleField];
    this.user = {
      firstName: this.formFields['firstName'].value,
      lastName: this.formFields['lastName'].value,
      age: this.formFields['age'].value,
      phoneNumber: this.formFields['phone'].value,
      email: this.formFields['email'].value,
      password: this.formFields['password'].value,
      role: role,
      specialization: specialization,
      clinicId: clinicId
    }

    this.addUser(this.user);
  }

  addUser(user: IUserToAdd, formDirective?: FormGroupDirective): void {
    this.userService.addUser(user)
      .subscribe({
        next: () => {
          this.snackBar.open(`User ${user.firstName} ${user.lastName} was created with success!`, '', { duration: 3000 });
          if (formDirective != undefined) {
            formDirective.resetForm();
          }
          this.submitted = false;
          this.addUserForm.reset();

        },
        error: (err) => {
          if (err.status === 0) {
            this.errorMessage = 'An error occured and the user could not be created! Please try again!';
          }
          else {
            this.errorMessage = err;
          }
        }
      })
  }

}
