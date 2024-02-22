import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { IUser, UserRole } from '../../../models/IUser';
import { UserService } from '../../../users/service/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  userList: IUser[] = [];
  selectedPatientId!: number;
  errorMessage: string = '';
  searchUserForm = this.formBuilder.group({
    firstNameInput: ['', Validators.required],
    lastNameInput: ['', Validators.required]
  });

  constructor(private userService: UserService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getAllUsers();
  }

  onSubmit(): void {
    this.getUserByName(this.searchUserForm.value.firstNameInput, this.searchUserForm.value.lastNameInput);
    if (this.userList.length == 0) {
      this.errorMessage = "No user with the given name!";
      this.getAllUsers();
    }
    this.searchUserForm.reset();
  }

  getUserByName(firstName: string, lastName: string): void {
    this.userService.getUserByName(firstName, lastName).subscribe({
      next: (res) => {
        this.errorMessage = "";
        this.userList = res;
      },
      error: (err) => {
        this.errorMessage = err;
      }
    }
    )
  }
  
  getAllUsers() {
    this.userService.getAll().subscribe({
      next: (res) => {
        this.errorMessage = "";
        this.userList = res;
        this.searchUserForm.reset();
      },
      error: (err) => {
        console.log(err);
      }
    }
    )
  }

  public get UserRole() {
    return UserRole;
  }
}
