import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IUser, IUserToAdd, UserRole } from '../models/IUser';
import { RegisterService } from './service/register.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '../users/service/user.service';
import { Specializations } from '../models/IMedic';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  loginForm!: FormGroup;
  submitted = false;
  errorMessage = '';
  acountCreated = false;
  user!: IUserToAdd;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      age: [''],
      phone: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      password: ['', Validators.required],
    });
  }

  get formFields() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }
    this.user = {
      firstName: this.formFields['firstName'].value,
      lastName: this.formFields['lastName'].value,
      age: this.formFields['age'].value,
      phoneNumber: this.formFields['phone'].value,
      email: this.formFields['email'].value,
      password: this.formFields['password'].value,
      role: UserRole.Patient,
      specialization: Specializations.Undefined,
      clinicId: undefined

    }

    this.addUser(this.user);
    this.loginForm.reset();
    this.router.navigate(['/login']);
  }

  addUser(user: IUserToAdd): void {
    this.userService.addUser(user)
      .subscribe({
        next: (res) => {
          this.snackBar.open(`User ${user.firstName} ${user.lastName} was created with success!`, '', { duration: 3000 });
        },
        error: (err) => {
          this.errorMessage = err;
        }
      })
  }
}
