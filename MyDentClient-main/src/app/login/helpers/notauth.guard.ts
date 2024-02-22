import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../service/authentication.service';


@Injectable({ providedIn: 'root' })
export class NotAuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(): boolean {
        if (this.authenticationService.isAuthenticated()) {
            this.router.navigate(['/home']);
            return false;
        }
        return true;
    }
}