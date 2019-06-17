import { Injectable } from '@angular/core';
// tslint:disable-next-line:max-line-length
import { CanActivate, Router} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private aletify: AlertifyService) {}

  canActivate(): boolean {

    if (this.authService.loggedIn()) {
      return true;
    }

    this.aletify.error('Ypu are not allowed to pass!');
    this.router.navigate(['/home']);

    return false;
  }

}
