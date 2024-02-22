import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../users/service/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm!: FormGroup;
  isSubmitted: boolean = false;
  errorMessage: string = '';

  constructor(private formBuilder: FormBuilder, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.changePasswordForm = this.formBuilder.group({
      newPassword: ['', Validators.required, Validators.minLength(6),
      Validators.maxLength(12)],
      confirmPassword: ['', Validators.required]
    }, {
      validator: this.passwordsShouldMatch
    });
  }
  

  get formFields() { return this.changePasswordForm.controls; }

  public passwordsShouldMatch(group: FormGroup) {
    let pass = group.controls['newPassword'].value;
    let confirmPass = group.controls['confirmPassword'].value;
    return pass === confirmPass ? null : { notSame: true }
}

  onSubmit(): void {
    this.errorMessage = "";
    this.isSubmitted = true;

    if (this.changePasswordForm.invalid) {
      return;
    }
    this.userService.changePassword(this.formFields['confirmPassword'].value)
      .subscribe({
        next: (res) => {
          //this.openModal();
        },
        error: (err) => {
          this.errorMessage = err;
        }
      });
  }

  cancelChangePassword(): void {
    this.router.navigateByUrl('/home');
  }
}
