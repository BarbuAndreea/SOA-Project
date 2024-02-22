import { Component, OnInit } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';
import { AuthenticationService } from '../login/service/authentication.service';
import { UserRole } from '../models/IUser';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  currentUserRole: UserRole = UserRole.Patient;
  currentUserFirstName: string = "";
  currentUserLastName: string = "";
  
  constructor(private authenticationService: AuthenticationService, private router: Router) {
  }

  ngOnInit(): void {
    this.currentUserRole = this.authenticationService.currentUserValue.role;
    this.currentUserFirstName = this.authenticationService.currentUserValue.firstName;
    this.currentUserLastName = this.authenticationService.currentUserValue.lastName;
  }

  public get userRole() {
    return UserRole;
  }

  logout() {
    this.authenticationService.logout();
  }
}
